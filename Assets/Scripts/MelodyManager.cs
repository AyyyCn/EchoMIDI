using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MelodyManager : MonoBehaviour
{
    /* ──────────────────────────── Références UI / Audio ──────────────────────────── */
    [Header("Melody Setup")]
    public AudioSource        audioSource;
    public KeyboardVisualizer keyboard;          // script qui éclaire les touches
    public GameObject         inputWindowUI;     // zone qui apparaît pendant la saisie
    public TextMeshProUGUI    scoreText;
    public Image              timerFillImage;

    [Header("Feedback")]
    public TextMeshProUGUI feedbackText;         // « Memorize / Play / Mistake / Correct »
    [SerializeField] private float feedbackDuration = 1f;

    /* ──────────────────────────── Paramètres (surchargés par config) ─────────────── */
    [Header("Timings")]
    [SerializeField] private float delayBetweenNotes = 0.6f;
    [SerializeField] private float inputTimeout      = 4f;

    [Header("Difficulty / Options")]
    public  int   sequenceLength           = 3;
    public  int   maxLength                = 10;
    public  int   step                     = 1;
    public  bool  enableVelocityThreshold  = false;
    public  float velocityMin              = 0.4f;
    public  float velocityMax              = 1.0f;
    public  bool  skipResetOnWrongInput    = false;

    /* ──────────────────────────── État interne ──────────────────────────── */
    private int       currentIndex;
    private bool      inputEnabled;
    private Coroutine inputTimerCoroutine;

    public  List<NoteStep> fullMelody      = new();   // mélodie complète chargée depuis json
    private List<NoteStep> currentSequence = new();   // séquence courante à reproduire

    private int score, streak, maxStreak;
    private Color feedbackColor;

    /* ──────────────────────────── Structures auxiliaires ─────────────────────────── */
    [System.Serializable] public class NoteData      { public int midiNote;   public string clipName; }
    [System.Serializable] private class NoteWrapper { public List<NoteData> notes; }

    /* ──────────────────────────── Initialisation ─────────────────────────── */
    private void Awake()
    {
        // Récupère la configuration envoyée par le menu
        GameConfig cfg           = MainMenuManager.config;   // ne sera jamais null
        delayBetweenNotes        = cfg.delayBetweenNotes;
        inputTimeout             = cfg.inputTimeout;
        sequenceLength           = cfg.minNotes;
        maxLength                = cfg.maxNotes;
        step                     = cfg.step;
        enableVelocityThreshold  = cfg.enableVelocityThreshold;
        velocityMin              = cfg.minVelocity;
        velocityMax              = cfg.maxVelocity;
        skipResetOnWrongInput    = cfg.skipResetOnWrongInput;
    }

    private void Start()
    {
        feedbackColor  = feedbackText.color;
        fullMelody     = LoadMelodyFromJson("melody");
        PrepareCurrentSequence();
        StartCoroutine(PlaySequence());
    }

    private void OnEnable () => MidiInputManager.OnNotePressed += HandleNoteInput;
    private void OnDisable() => MidiInputManager.OnNotePressed -= HandleNoteInput;

    /* ──────────────────────────── Préparation séquence ─────────────────────────── */
    private void PrepareCurrentSequence()
    {
        currentSequence.Clear();
        int length = Mathf.Min(sequenceLength, fullMelody.Count);
        for (int i = 0; i < length; ++i)
            currentSequence.Add(fullMelody[i]);

        currentIndex = 0;
    }

    /* ──────────────────────────── Lecture de la séquence (IA) ───────────────────── */
    private IEnumerator PlaySequence()
    {
        feedbackText.color = feedbackColor;
        feedbackText.SetText("Memorize the notes!");
        yield return new WaitForSeconds(1f);

        PrepareCurrentSequence();

        inputEnabled = false;
        inputWindowUI.SetActive(false);
        ResetTimerVisual();
        yield return new WaitForSeconds(0.5f);

        foreach (NoteStep step in currentSequence)
        {
            audioSource.PlayOneShot(step.noteSound);
            keyboard.HighlightNote(step.midiNote);
            yield return new WaitForSeconds(delayBetweenNotes);
        }

        inputEnabled = true;
        inputWindowUI.SetActive(true);
        feedbackText.color = feedbackColor;
        feedbackText.SetText("Play!");
        StartOrRestartInputTimer();
    }

    /* ──────────────────────────── Gestion des frappes MIDI ──────────────────────── */
    private void HandleNoteInput(int inputNote, float velocity)
    {
        if (!inputEnabled || currentIndex >= currentSequence.Count) return;
        if (enableVelocityThreshold && (velocity < velocityMin || velocity > velocityMax)) return;

        NoteStep expected = currentSequence[currentIndex];

        if (inputNote == expected.midiNote)   // ✅ bonne note
        {
            audioSource.PlayOneShot(expected.noteSound, velocity);
            keyboard.HighlightNote(inputNote, velocity);
            currentIndex++;

            if (currentIndex == currentSequence.Count)          // séquence terminée
            {
                StartCoroutine(HandleSequenceComplete());
            }
            else
            {
                StartOrRestartInputTimer();
            }
        }
        else                                   // ❌ mauvaise note
        {
            feedbackText.color = Color.red;
            feedbackText.SetText("Mistake!");
            streak = 0;
            UpdateScoreText();

            if (skipResetOnWrongInput)
            {
                StartOrRestartInputTimer();
            }
            else
            {
                StartCoroutine(ResetAndReplay());
            }
        }
    }

    /* ──────────────────────────── Séquence réussie ─────────────────────────── */
    private IEnumerator HandleSequenceComplete()
    {
        feedbackText.color = Color.green;
        feedbackText.SetText("Correct!");

        score++;
        streak++;
        maxStreak = Mathf.Max(maxStreak, streak);
        UpdateScoreText();

        inputEnabled = false;
        inputWindowUI.SetActive(false);
        StopInputTimer();

        if (sequenceLength < maxLength) sequenceLength += step;

        yield return new WaitForSeconds(feedbackDuration);
        StartCoroutine(PlaySequence());
    }

    /* ──────────────────────────── Timer d’entrée ─────────────────────────── */
    private void StartOrRestartInputTimer()
    {
        StopInputTimer();
        inputTimerCoroutine = StartCoroutine(InputTimer());
    }

    private void StopInputTimer()
    {
        if (inputTimerCoroutine != null)
        {
            StopCoroutine(inputTimerCoroutine);
            inputTimerCoroutine = null;
        }
        ResetTimerVisual();
    }

    private IEnumerator InputTimer()
    {
        float elapsed = 0f;
        while (elapsed < inputTimeout)
        {
            elapsed += Time.deltaTime;
            if (timerFillImage) timerFillImage.fillAmount = 1f - (elapsed / inputTimeout);
            yield return null;
        }
        StartCoroutine(ResetAndReplay());
    }

    private void ResetTimerVisual()
    {
        if (timerFillImage) timerFillImage.fillAmount = 0f;
    }

    /* ──────────────────────────── Réinitialisation après erreur ─────────── */
    private IEnumerator ResetAndReplay()
    {
        inputEnabled = false;
        inputWindowUI.SetActive(false);
        StopInputTimer();

        currentIndex = 0;
        streak       = 0;
        UpdateScoreText();

        feedbackText.color = feedbackColor;
        feedbackText.SetText("Memorize the notes!");
        yield return new WaitForSeconds(1f);

        StartCoroutine(PlaySequence());
    }

    /* ──────────────────────────── Chargement des données ─────────────────── */
    private List<NoteStep> LoadMelodyFromJson(string filename)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(filename);
        if (jsonFile == null)
        {
            Debug.LogError($"❌ Could not load {filename}.json from Resources.");
            return new List<NoteStep>();
        }

        // On emballe le tableau JSON dans un objet pour JsonUtility
        var wrapper = JsonUtility.FromJson<NoteWrapper>("{\"notes\":" + jsonFile.text + "}");
        List<NoteStep> result = new();

        foreach (NoteData note in wrapper.notes)
        {
            result.Add(new NoteStep
            {
                midiNote  = note.midiNote,
                noteSound = LoadClip(note.clipName)
            });
        }
        return result;
    }

    private AudioClip LoadClip(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>($"wav/{name}");
        if (clip == null) Debug.LogError($"❌ Missing clip: wav/{name}");
        return clip;
    }

    /* ──────────────────────────── Affichage du score ─────────────────────── */
    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}   Streak: {streak}   Max: {maxStreak}";
    }
}

