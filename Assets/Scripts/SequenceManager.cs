using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    public List<int> CurrentSequence { get; private set; } = new();

    [Header("Note Range")]
    public int minNote = 60; // C4
    public int maxNote = 72; // C5

    [Header("Gameplay Settings")]
    public int initialLength = 3;

    private System.Random rng = new();

    void Start()
    {
        GenerateNewSequence(initialLength);
    }

    public void GenerateNewSequence(int length)
    {
        CurrentSequence.Clear();
        for (int i = 0; i < length; i++)
        {
            int note = rng.Next(minNote, maxNote + 1);
            CurrentSequence.Add(note);
        }

        Debug.Log($"[SequenceManager] New sequence generated: {string.Join(", ", CurrentSequence)}");
    }

    public void AddNoteToSequence()
    {
        int newNote = rng.Next(minNote, maxNote + 1);
        CurrentSequence.Add(newNote);
        Debug.Log($"[SequenceManager] Added note {newNote} → Sequence: {string.Join(", ", CurrentSequence)}");
    }

    public void ResetSequence()
    {
        GenerateNewSequence(initialLength);
    }
}
