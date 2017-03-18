using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{
    private PooledObject[] m_prefabs;
    private Queue<PooledObject> m_pool;
    private Transform m_poolRoot;
    private int m_lastInstance;

    public ObjectPool(PooledObject[] prefabs, int startSize)
    {
        m_prefabs = prefabs;
        m_poolRoot = new GameObject(prefabs[0].name + "Pool").transform;

        m_lastInstance = Random.Range(0, m_prefabs.Length);

        m_pool = new Queue<PooledObject>();
        for (int i = 0; i < startSize; i++)
        {
            Deactivate(CreateInstance());
        }
    }
    
    public PooledObject GetObject(Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        PooledObject obj;
        if (m_pool.Count > 0)
        {
            obj = m_pool.Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.gameObject.SetActive(true);
        }
        else
        {
            obj = CreateInstance(position, rotation);
        }
        return obj;
    }

    private PooledObject CreateInstance(Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        PooledObject obj = GameObject.Instantiate(m_prefabs[m_lastInstance], position, rotation, m_poolRoot);
        m_lastInstance = (m_lastInstance + 1) % m_prefabs.Length;
        obj.SetPool(this);
        return obj;
    }

    public void Deactivate(PooledObject go)
    {
        go.gameObject.SetActive(false);
        m_pool.Enqueue(go);
    }

    public void ClearPool()
    {
        while (m_pool.Count > 0)
        {
            PooledObject obj = m_pool.Dequeue();
            GameObject.Destroy(obj.gameObject);
        }
    }
}
