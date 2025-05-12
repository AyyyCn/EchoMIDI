using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections;

public class NoteVisualizer : MonoBehaviour
{
    [SerializeField] private Image circleImage;
    [SerializeField] private TextMeshProUGUI noteLabel;

    public void SetNote(int midiNote)
    {
        noteLabel.text = midiNote.ToString();
    }

    public void Pulse()
    {
        StopAllCoroutines();
        StartCoroutine(PulseEffect());
    }

    private IEnumerator PulseEffect()
    {
        Color originalColor = circleImage.color;
        Vector3 originalScale = transform.localScale;

        circleImage.color = Color.yellow;
        transform.localScale = originalScale * 1.2f;

        yield return new WaitForSeconds(0.2f);

        circleImage.color = originalColor;
        transform.localScale = originalScale;
    }
    public void ResetVisual()
    {
        circleImage.color = Color.white;
        transform.localScale = Vector3.one;
    }

}
