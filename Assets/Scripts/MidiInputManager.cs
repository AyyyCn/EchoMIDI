using UnityEngine;
using MidiJack;
using System;

public class MidiInputManager : MonoBehaviour
{
    public static event Action<int, float> OnNotePressed;
    public static event Action<int> OnNoteReleased;

    public KeyboardVisualizer keyboard;

    void OnEnable()
    {
        MidiMaster.noteOnDelegate += HandleNoteOn;
        MidiMaster.noteOffDelegate += HandleNoteOff;
    }

    void OnDisable()
    {
        MidiMaster.noteOnDelegate -= HandleNoteOn;
        MidiMaster.noteOffDelegate -= HandleNoteOff;
    }

    private void HandleNoteOn(MidiChannel channel, int note, float velocity)
    {
        Debug.Log($"[MidiInputManager] Note ON: {note}, Velocity: {velocity}");
       
        OnNotePressed?.Invoke(note, velocity);
    }

    private void HandleNoteOff(MidiChannel channel, int note)
    {
        Debug.Log($"[MidiInputManager] Note OFF: {note}");
        OnNoteReleased?.Invoke(note);
    }
}
