using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private int m_num;
    private Color m_color;
    private Cursor m_cursor;
    private List<Turret> m_turrets;

    public Player(int num, Color color, Cursor cursor, List<Turret> turrets)
    {
        m_num = num;
        m_color = color;
        m_cursor = cursor;
        m_turrets = turrets;

        m_cursor.Init(m_color);
    }

    public void Update()
    {
        m_cursor.Move();
        foreach (Turret t in m_turrets)
        {
            Vector3 targetPos = m_cursor.GetTargetPos();
            t.StateUpdate(targetPos);
            if (Controls.Instance.IsDown(GameButton.Fire1))
            {
                t.FireBullet(targetPos);
            }
        }
    }
}
