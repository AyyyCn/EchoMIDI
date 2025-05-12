using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceDisplayManager : MonoBehaviour
{
    public SequenceManager sequenceManager;
    public KeyboardVisualizer keyboard;

    public void ShowSequence()
    {
        StartCoroutine(PlayVisualSequence());
    }

    private IEnumerator PlayVisualSequence()
    {
        // Create a copy to avoid modification during enumeration
        List<int> sequenceCopy = new(sequenceManager.CurrentSequence);

        foreach (int note in sequenceCopy)
        {
            keyboard.HighlightNote(note);
            yield return new WaitForSeconds(0.5f);
        }
    }

}
