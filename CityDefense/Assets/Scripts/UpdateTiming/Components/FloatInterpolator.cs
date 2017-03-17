using UnityEngine;

public class FloatInterpolator : MonoBehaviour, InterpolationComponent
{
    private Interpolator<float> m_interpolator;
    private InterpolatedFloat m_interpolated;
    private bool m_initialized = false;

    public Interpolator<float> Initialize(InterpolatedFloat interpolated, bool useThreshold = false)
    {
        m_interpolator = new Interpolator<float>(interpolated, useThreshold);
        m_interpolated = interpolated;
        m_initialized = true;
        return m_interpolator;
    }

    private void OnEnable()
    {
        InterpolationController.AddComponent(this);
    }

    private void OnDisable()
    {
        InterpolationController.RemoveComponent(this);
    }

    public void SetThreshold(float threshold)
    {
        m_interpolated.SetThreshold(threshold);
    }

    private void Start()
    {
        if (m_interpolator == null)
        {
            Debug.LogError("float interpolator is null on " + transform.name);
        }
        if (!m_initialized)
        {
            Debug.LogError("float interpolator was not initialized on " + transform.name);
        }
    }
    
    public void FixedFrame()
    {
        if (enabled)
        {
            m_interpolator.FixedUpdate();
        }
    }

    public void UpdateFrame(float factor)
    {
        if (enabled)
        {
            m_interpolator.Update(factor);
        }
    }
}