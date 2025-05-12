using UnityEngine;
using UnityEngine.UI;

public class Randomizer : MonoBehaviour
{
    public Sprite spriteA;
    public Sprite spriteB;

    private Image img;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    void Start()
    {
        if (spriteA == null || spriteB == null || img == null)
        {
            Debug.LogWarning("Missing sprites or Image component.");
            return;
        }

        // 50% chance to assign either sprite
        img.sprite = (Random.value < 0.5f) ? spriteA : spriteB;
    }
}
