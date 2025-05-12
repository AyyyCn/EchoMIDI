using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FakeKeyUI : MonoBehaviour
{
    public int midiNote;
    private Image img;
    public Color color1;
    public Color color2;
    public GameObject keyParticlePrefab;
    public GameObject starParticlePrefab;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    private Coroutine pulseCoroutine;

    public void Pulse(float velocity)
    {
        if (pulseCoroutine != null)
            StopCoroutine(pulseCoroutine);

        pulseCoroutine = StartCoroutine(PulseEffect(velocity));
        SpawnKeyParticle(velocity);

        // Optional: only spawn stars on strong press
        if (velocity > 0.8f)
            SpawnStarParticle();
    }

    private IEnumerator PulseEffect(float velocity)
    {
        //float scaleFactor = Mathf.Lerp(1f, 1.1f, velocity);
        Color flashColor = Color.Lerp(color1, color2, velocity);

        Color original = img.color;
        Vector3 originalScale = transform.localScale;

        flashColor.a = 1f;
        img.color = flashColor;
        transform.localScale = originalScale ;//* scaleFactor;

        yield return new WaitForSeconds(0.2f);

        img.color = original;
        transform.localScale = originalScale;
    }

    void SpawnKeyParticle(float velocity)
    {
        if (!keyParticlePrefab) return;

        var go = Instantiate(keyParticlePrefab, this.transform);
        RectTransform rt = go.GetComponent<RectTransform>();

        // Use localPosition instead of anchoredPosition
        rt.localPosition= new Vector3(0, 100f, 0);
        rt.localScale = Vector3.one * Mathf.Lerp(0.6f, 1.0f, velocity);
    }

    void SpawnStarParticle()
    {
        if (!starParticlePrefab) return;

        var go = Instantiate(starParticlePrefab, this.transform);
        RectTransform rt = go.GetComponent<RectTransform>();

        // Offset using localPosition instead of anchoredPosition
        rt.localPosition = new Vector3(Random.Range(-1f,1f)*100, 300f+Random.Range(-1f,1f)*100, 0);
        rt.localScale = Vector3.one * Random.Range(0.5f, 0.7f);
    }
}
