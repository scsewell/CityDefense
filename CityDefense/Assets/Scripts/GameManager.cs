using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Target m_city;
    public Target City
    {
        get { return m_city; }
    }

    [SerializeField]
    private Target m_factory;
    public Target Factory
    {
        get { return m_factory; }
    }

    private bool m_roundOver = false;
    public bool RoundOver
    {
        get { return m_roundOver; }
    }

    private float m_roundTime = 0;
    public float RoundTime
    {
        get { return m_roundTime; }
    }

    private float m_timeScale = 1;

    private void Update()
    {
        GameUI.Instance.UpdateTime(m_roundTime);
        
        if (GameMenu.Instance.IsMenuOpen())
        {
            Time.timeScale = 0;
        }
        else
        {
            m_timeScale = m_roundOver ? Mathf.Lerp(m_timeScale, 0, 0.05f) : 1;
            if (m_timeScale < 0.01f)
            {
                m_timeScale = 0;
            }
            Time.timeScale = m_timeScale;
        }
    }

    private void FixedUpdate()
    {
        if (!m_city.GetComponent<Health>().IsAlive)
        {
            m_roundOver = true;
        }

        if (!m_roundOver)
        {
            m_roundTime += Time.deltaTime;
        }
    }
}
