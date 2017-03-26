using UnityEngine;
using UnityEngine.UI;
using System;

public class PanelSlider : MonoBehaviour, ISettingPanel
{
    public Text label;
    public Text valueText;
    public Slider slider;
    
    private bool m_intOnly;
    private Func<float> m_get;
    private Action<float> m_set;

    public RectTransform Init(string name, Func<float> get, Action<float> set, float min, float max, bool intOnly)
    {
        m_get = get;
        m_set = set;
        label.text = name;
        slider.minValue = min;
        slider.maxValue = max;
        m_intOnly = intOnly;
        slider.wholeNumbers = intOnly;

        return GetComponent<RectTransform>();
    }

    public void Load()
    {
        slider.value = m_get();
        UpdateText();
    }

    public void Apply()
    {
        m_set(slider.value);
    }

    public void OnValueChanged()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        valueText.text = m_intOnly ? slider.value.ToString() : (Mathf.Round(slider.value * 100) / 100.0f).ToString();
    }
}
