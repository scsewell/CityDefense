using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class GameUI : Singleton<GameUI>
{
    [SerializeField]
    private Crosshair m_crosshairPrefab;
    [SerializeField]
    private Text m_scoreText;

    private Transform m_crosshairs;
    private Transform m_healthbars;

    private StringBuilder m_scoreBuilder;

    private void Awake()
    {
        m_crosshairs = new GameObject("Crosshairs").transform;
        m_crosshairs.SetParent(transform, false);

        m_healthbars = new GameObject("Healthbars").transform;
        m_healthbars.SetParent(transform, false);

        m_scoreBuilder = new StringBuilder("$", 10);
    }

    public Crosshair AddCrosshair()
    {
        return Instantiate(m_crosshairPrefab, m_crosshairs, false);
    }

    public Healthbar AddHealthbar(Healthbar healthbar)
    {
        return Instantiate(healthbar, m_healthbars, false);
    }

    public void UpdateMoney(int money)
    {
        m_scoreBuilder.Remove(1, m_scoreBuilder.Length - 1);
        m_scoreBuilder.Append(money);
        m_scoreText.text = m_scoreBuilder.ToString();
    }
}
