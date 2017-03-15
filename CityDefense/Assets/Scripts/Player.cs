using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private Color m_color;
    private Transform m_cursor;

    public Player(Color color, Transform cursor)
    {
        m_color = color;
        m_cursor = cursor;
    }

    private void Update()
    {

    }
}
