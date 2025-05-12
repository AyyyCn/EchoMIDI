using System.Collections.Generic;
using UnityEngine;

public class PlayerInputTracker : MonoBehaviour
{
    [Header("References")]
    public SequenceManager sequenceManager;

    public System.Action OnSuccess;
    public System.Action OnFailure;

    private List<int> currentTargetSequence = new();
    private int currentIndex = 0;

    void OnEnable()
    {
        MidiInputManager.OnNotePressed += HandleNote;
    }

    void OnDisable()
    {
        MidiInputManager.OnNotePressed -= HandleNote;
    }

    public void StartListening()
    {
        currentTargetSequence = new List<int>(sequenceManager.CurrentSequence);
        currentIndex = 0;
        Debug.Log("[InputTracker] Player is now listening...");
    }

    private void HandleNote(int note, float velocity)
    {
        if (currentIndex >= currentTargetSequence.Count)
            return; // Sequence already completed

        int expectedNote = currentTargetSequence[currentIndex];

        if (note == expectedNote)
        {
            Debug.Log($"✅ Correct: {note}");
            currentIndex++;

            if (currentIndex == currentTargetSequence.Count)
            {
                Debug.Log("🎉 Full sequence matched!");
                OnSuccess?.Invoke();
            }
        }
        else
        {
            Debug.Log($"❌ Wrong note: {note} (expected {expectedNote})");
            OnFailure?.Invoke();
            // Optionally: reset, disable input, etc.
        }
    }
}
