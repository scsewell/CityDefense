using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class DebugDisplay : MonoBehaviour
{
    public float fpsUpdateInterval = 0.5f;

    private int m_framesOverInterval = 0;
    private float m_fpsOverInterval = 0f;
    private float m_timeLeftBeforeUpdate;
    private float m_fps;

    private Text m_text;
    private StringBuilder m_sb;
    
    private void Awake()
    {
        m_text = GetComponent<Text>();
        m_sb = new StringBuilder();
    }

    private void Start()
    {
        m_timeLeftBeforeUpdate = fpsUpdateInterval;
    }

    private void LateUpdate()
    {
        m_sb.Remove(0, m_sb.Length);

        // update fps
        m_fpsOverInterval += 1 / Time.deltaTime;
        m_framesOverInterval++;

        m_timeLeftBeforeUpdate -= Time.deltaTime;
        if (m_timeLeftBeforeUpdate <= 0)
        {
            m_fps = (m_fpsOverInterval / m_framesOverInterval);
            m_timeLeftBeforeUpdate = fpsUpdateInterval;
            m_fpsOverInterval = 0;
            m_framesOverInterval = 0;

            // set display information
            if (Settings.Instance.GetShowFPS())
            {
                m_sb.Append("fps: ");
                m_sb.Append(m_fps);
                m_sb.Append("\n");
            }

            m_text.text = m_sb.ToString();
        }
    }
}
