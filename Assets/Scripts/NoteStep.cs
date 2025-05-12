using UnityEngine;
[System.Serializable]
public class NoteStep
{
    public int midiNote;           // MIDI note to expect (e.g. 60 = C4)
    public AudioClip noteSound;    // The sound to play when it's correctly hit
}
