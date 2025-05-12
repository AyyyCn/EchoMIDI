using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;

    [Header("Settings UI")]
    public Slider          delaySlider;
    public Slider          timeoutSlider;
    public TMP_InputField  minNotesInput;
    public TMP_InputField  maxNotesInput;
    public TMP_InputField  stepInput;
    public Slider          minVelocitySlider;
    public Slider          maxVelocitySlider;
    public Toggle          skipRepeatToggle;
    public Toggle          velocityThresholdToggle;   // ← nouveau si tu veux l’activer/désactiver

    // Config statique lue par MelodyManager
    public static GameConfig config = new GameConfig();

    /* ────────── Boutons du menu ────────── */
    public void OnPlayGuided() => SceneManager.LoadScene("GameScene");
    public void OnPlayFree()   => Debug.Log("Free Mode: Coming soon!");

    public void OnOpenSettings()  => settingsPanel.SetActive(true);
    public void OnCloseSettings() => settingsPanel.SetActive(false);

    public void OnApplySettings()
    {
        config.delayBetweenNotes        = delaySlider.value;
        config.inputTimeout             = timeoutSlider.value;
        config.minNotes                 = int.Parse(minNotesInput.text);
        config.maxNotes                 = int.Parse(maxNotesInput.text);
        config.step                     = int.Parse(stepInput.text);
        config.minVelocity              = minVelocitySlider.value;
        config.maxVelocity              = maxVelocitySlider.value;
        config.enableVelocityThreshold  = false;
        config.skipResetOnWrongInput    = skipRepeatToggle.isOn;
        OnCloseSettings();
    }
}

[System.Serializable]
public class GameConfig
{
    public float delayBetweenNotes       = 0.6f;
    public float inputTimeout            = 4f;
    public int   minNotes                = 3;
    public int   maxNotes                = 10;
    public int   step                    = 1;
    public float minVelocity             = 0.1f;
    public float maxVelocity             = 1.0f;
    public bool  enableVelocityThreshold = false;
    public bool  skipResetOnWrongInput   = false;
}
