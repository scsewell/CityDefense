using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    private static Singleton m_instance;
    public static Singleton Instance
    {
        get { return m_instance; }
        set { m_instance = value; }
    }

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Debug.LogWarning("Singleton already already exists!");
            Destroy(this);
        }
    }
}
