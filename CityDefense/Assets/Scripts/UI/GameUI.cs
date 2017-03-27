using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class GameUI : Singleton<GameUI>
{
    [SerializeField]
    private Crosshair m_crosshairPrefab;

    [SerializeField]
    private Text m_timeText;
    [SerializeField]
    private Text m_scoreText;
    [SerializeField]
    private Text m_moneyText;
    [SerializeField]
    private Text m_ammoBullet1Text;

    private Transform m_crosshairs;
    private Transform m_healthbars;

    private StringBuilder m_timeBuilder;
    private StringBuilder m_scoreBuilder;
    private StringBuilder m_moneyBuilder;
    private StringBuilder m_ammoBullet1Builder;
    
    private int m_lastSeconds;

    private void Awake()
    {
        m_crosshairs = new GameObject("Crosshairs").transform;
        m_crosshairs.SetParent(transform, false);

        m_healthbars = new GameObject("Healthbars").transform;
        m_healthbars.SetParent(transform, false);

        m_timeBuilder = new StringBuilder("0:00", 16);
        m_scoreBuilder = new StringBuilder("0", 16);
        m_moneyBuilder = new StringBuilder("$", 16);
        m_ammoBullet1Builder = new StringBuilder("Bullets: ", 16);
    }

    private void Update()
    {
        if (GameManager.Instance.RoundOver)
        {
            m_crosshairs.gameObject.SetActive(false);
        }
    }

    public Crosshair AddCrosshair()
    {
        return Instantiate(m_crosshairPrefab, m_crosshairs, false);
    }

    public Healthbar AddHealthbar(Healthbar healthbar)
    {
        return Instantiate(healthbar, m_healthbars, false);
    }

    public void UpdateTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time - (minutes * 60));

        if (seconds != m_lastSeconds)
        {
            m_timeBuilder.Remove(0, m_timeBuilder.Length);
            m_timeBuilder.Append(minutes);
            m_timeBuilder.Append(":");
            if (seconds < 10)
            {
                m_timeBuilder.Append("0");
            }
            m_timeBuilder.Append(seconds);
            m_timeText.text = m_timeBuilder.ToString();
            m_lastSeconds = seconds;
        }
    }

    public void UpdateScore(int score)
    {
        m_scoreBuilder.Remove(0, m_scoreBuilder.Length);
        m_scoreBuilder.Append(score);
        m_scoreText.text = m_scoreBuilder.ToString();
    }

    public void UpdateMoney(int money)
    {
        m_moneyBuilder.Remove(1, m_moneyBuilder.Length - 1);
        m_moneyBuilder.Append(money);
        m_moneyText.text = m_moneyBuilder.ToString();
    }

    public void UpdateAmmoBullet1(int ammo)
    {
        m_ammoBullet1Builder.Remove(9, m_ammoBullet1Builder.Length - 9);
        m_ammoBullet1Builder.Append(ammo);
        m_ammoBullet1Text.text = m_ammoBullet1Builder.ToString();
    }
}
