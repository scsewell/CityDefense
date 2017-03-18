using UnityEngine;
using System.Collections.Generic;

public class InterpolationController : Singleton
{
    private static List<IInterpComponent> m_components;
    
    private float m_lastFixedTime;

    private void Awake()
    {
        m_components = new List<IInterpComponent>();
    }

    private void FixedUpdate()
    {
        m_lastFixedTime = Time.time;
        foreach (IInterpComponent component in m_components)
        {
            component.FixedFrame();
        }
    }

    private void Update()
    {
        float factor = (Time.time - m_lastFixedTime) / Time.fixedDeltaTime;
        foreach (IInterpComponent component in m_components)
        {
            component.UpdateFrame(factor);
        }
    }

    public static void AddComponent(IInterpComponent component)
    {
        if (!m_components.Contains(component))
        {
            m_components.Add(component);
        }
    }

    public static void RemoveComponent(IInterpComponent component)
    {
        m_components.Remove(component);
    }
}
