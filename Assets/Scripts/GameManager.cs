using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    public SequenceManager sequenceManager;
    public PlayerInputTracker inputTracker;
    public SequenceDisplayManager sequenceDisplay;

    [Header("Settings")]
    public int initialSequenceLength = 3;

    private void Start()
    {
        // Set up listeners
        inputTracker.OnSuccess += HandleSuccess;
        inputTracker.OnFailure += HandleFailure;

        StartNewGame();
    }

    private void StartNewGame()
    {
        sequenceManager.GenerateNewSequence(initialSequenceLength);
        PlayRound();
    }

    private void PlayRound()
    {
        Debug.Log("🎬 Showing new sequence...");
        sequenceDisplay.ShowSequence();
        StartCoroutine(DelayedStartListening());
    }

    private IEnumerator DelayedStartListening()
    {
        float delay = sequenceManager.CurrentSequence.Count * 0.5f + 0.5f;
        yield return new WaitForSeconds(delay);
        Debug.Log("🎧 Start listening...");
        inputTracker.StartListening();
    }

    private void HandleSuccess()
    {
        Debug.Log("✅ Correct! Extending sequence...");
        sequenceManager.AddNoteToSequence();
        PlayRound();
    }

    private void HandleFailure()
    {
        Debug.Log("❌ Incorrect! Restarting sequence...");
        sequenceManager.GenerateNewSequence(initialSequenceLength);
        PlayRound();
    }
}
    