using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static object m_lock = new object();
    private static bool m_destroyed = false;

    private static T m_instance;
    public static T Instance
    {
        get
        {
            if (m_destroyed)
            {
                return null;
            }
            lock (m_lock)
            {
                if (m_instance == null)
                {
                    m_instance = (T)FindObjectOfType(typeof(T));
                }
                return m_instance;
            }
        }
    }
    
    protected virtual void OnDestroy()
    {
        m_destroyed = true;
    }
}
