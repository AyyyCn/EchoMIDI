using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseTester : MonoBehaviour
{
    public NoteVisualizer visual;

    void Start()
    {
        visual.SetNote(64); // example: E4
        visual.Pulse();     // simulate playing the note
    }
}
