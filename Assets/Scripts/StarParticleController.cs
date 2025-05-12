using UnityEngine;

public class StarParticleController : MonoBehaviour
{
    public float floatSpeed = 200f; // Now in UI units
    public float lifetime = 8f;

    private Vector2 drift;
    private float time;
    private CanvasGroup group;
    private RectTransform rt;

    void Start()
    {
        group = GetComponent<CanvasGroup>();
        rt = GetComponent<RectTransform>();

        // Pure upward + slight X drift
        drift = new Vector2(Random.Range(-0.6f, 0.6f), 1f);
    }

    void Update()
    {
        rt.anchoredPosition += drift * floatSpeed * Time.deltaTime;

        time += Time.deltaTime;

        if (time > lifetime - 2f)
            group.alpha = Mathf.Lerp(1f, 0f, (time - (lifetime - 2f)) / 2f);

        if (time >= lifetime)
            Destroy(gameObject);
    }
}
