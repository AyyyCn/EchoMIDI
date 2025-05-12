using UnityEngine;
using System.Collections.Generic;

public class KeyboardVisualizer : MonoBehaviour
{
    public Transform whiteKeyParent;
    public Transform blackKeyParent;
    public GameObject whiteKeyPrefab;
    public GameObject blackKeyPrefab;

    private readonly int[] whiteNotes = { 60, 62, 64, 65, 67, 69 };
    private readonly int[] blackNotes = { 61, 63, 66, 68};
    private readonly float[] blackNoteOffsets = { 0, 40, 120, 160, 200 }; // manual positioning

    private Dictionary<int, FakeKeyUI> noteToKey = new();

    void Start()
    {
        foreach (int note in whiteNotes)
        {
            var go = Instantiate(whiteKeyPrefab, whiteKeyParent);
            var key = go.GetComponent<FakeKeyUI>();
            key.midiNote = note;
            noteToKey[note] = key;
        }

        for (int i = 0; i < blackNotes.Length; i++)
        {
            int note = blackNotes[i];
            var go = Instantiate(blackKeyPrefab, blackKeyParent);
            var rect = go.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(blackNoteOffsets[i], 40);
            var key = go.GetComponent<FakeKeyUI>();
            key.midiNote = note;
            noteToKey[note] = key;
        }
    }

    public void HighlightNote(int midiNote, float velocity = 1f)
    {
        if (noteToKey.TryGetValue(midiNote, out var key))
            key.Pulse(velocity);
    }
}
