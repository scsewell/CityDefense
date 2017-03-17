using UnityEngine;
using System.Collections.Generic;

public class InterpolationController : Singleton
{
    private static List<InterpolationComponent> m_components;
    
    private float m_lastFixedTime;

    private void Awake()
    {
        m_components = new List<InterpolationComponent>();
    }

    private void FixedUpdate()
    {
        Time.fixedDeltaTime = 1.0f / 50;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 10;

        m_lastFixedTime = Time.time;
        foreach (InterpolationComponent component in m_components)
        {
            component.FixedFrame();
        }
    }

    private void Update()
    {
        float factor = (Time.time - m_lastFixedTime) / Time.fixedDeltaTime;
        foreach (InterpolationComponent component in m_components)
        {
            component.UpdateFrame(factor);
        }
    }

    public static void AddComponent(InterpolationComponent component)
    {
        if (!m_components.Contains(component))
        {
            m_components.Add(component);
        }
    }

    public static void RemoveComponent(InterpolationComponent component)
    {
        m_components.Remove(component);
    }
}
