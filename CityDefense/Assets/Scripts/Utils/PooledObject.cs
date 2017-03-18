using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private ObjectPool m_pool;

    public void SetPool(ObjectPool pool)
    {
        m_pool = pool;
    }

    public void Deactivate()
    {
        m_pool.Deactivate(this);
    }
}
