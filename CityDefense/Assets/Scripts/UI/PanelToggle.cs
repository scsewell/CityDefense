using UnityEngine;
using UnityEngine.UI;
using System;

public class PanelToggle : MonoBehaviour, ISettingPanel
{
    public Text label;
    public Toggle toggle;

    private Func<bool> m_get;
    private Action<bool> m_set;

    public RectTransform Init(string name, Func<bool> get, Action<bool> set)
    {
        m_get = get;
        m_set = set;
        label.text = name;

        return GetComponent<RectTransform>();
    }

    public void Load()
    {
        toggle.isOn = m_get();
    }

    public void Apply()
    {
        m_set(toggle.isOn);
    }
}
