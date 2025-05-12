using UnityEngine;

public class KeyParticleController : MonoBehaviour
{
    public float floatSpeed = 80f;
    public float lifetime = 0.8f;
    public float fadeStart = 0.5f;

    private float time;
    private CanvasGroup group;

    void Start()
    {
        group = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
        time += Time.deltaTime;

        if (time > fadeStart)
            group.alpha = Mathf.Lerp(1f, 0f, (time - fadeStart) / (lifetime - fadeStart));

        if (time >= lifetime)
            Destroy(gameObject);
    }
}
