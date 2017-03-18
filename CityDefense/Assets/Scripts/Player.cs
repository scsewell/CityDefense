using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private int m_score;
    public int Score
    {
        get { return m_score; }
    }

    private int m_num;
    private Color m_color;
    private Crosshair m_cursor;
    private List<Turret> m_turrets;

    public Player(int num, Color color, Crosshair cursor, List<Turret> turrets)
    {
        m_num = num;
        m_color = color;
        m_cursor = cursor;
        m_turrets = turrets;

        foreach (Turret t in m_turrets)
        {
            t.Init(this);
        }

        m_cursor.Init(m_color, m_num == 0);
    }

    public void Update()
    {
        m_cursor.Move();

        bool fire = Controls.Instance.IsDown(GameButton.Fire1);
        foreach (Turret t in m_turrets)
        {
            Vector3 targetPos = m_cursor.GetTargetPos();
            t.StateUpdate(targetPos);
            if (fire)
            {
                t.FireBullet(targetPos);
            }
        }
    }

    public void IncreaseScore(int score)
    {
        m_score += score;
    }
}
