using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueDisplay : MonoBehaviour
{
    public enum DisplayMode { None, Seconds, Percentage, Custom }

    [Header("References")]
    public Slider slider;
    public TMP_Text valueText;

    [Header("Display Options")]
    public DisplayMode mode = DisplayMode.None;
    public string customFormat = "{0}";

    void Start()
    {
        if (slider == null || valueText == null)
        {
            Debug.LogWarning("Slider or ValueText not assigned.");
            return;
        }

        slider.onValueChanged.AddListener(UpdateValue);
        UpdateValue(slider.value);
    }

    void UpdateValue(float value)
    {
        switch (mode)
        {
            case DisplayMode.Seconds:
                valueText.text = $"{value:F1} s";
                break;
            case DisplayMode.Percentage:
                valueText.text = $"{value * 100:F0}%";
                break;
            case DisplayMode.Custom:
                valueText.text = string.Format(customFormat, value);
                break;
            default:
                valueText.text = value.ToString("0.0");
                break;
        }
    }
}
