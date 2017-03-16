using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{
    private GameObject m_prefab;
    private Queue<GameObject> m_pool;
    private Transform m_poolRoot;

    public ObjectPool(GameObject prefab, int startSize)
    {
        m_prefab = prefab;

        m_poolRoot = new GameObject(prefab.name + "Pool").transform;

        m_pool = new Queue<GameObject>();
        for (int i = 0; i < startSize; i++)
        {
            GameObject obj = GameObject.Instantiate(m_prefab);
            obj.transform.SetParent(m_poolRoot);
            obj.SetActive(false);
            m_pool.Enqueue(obj);
        }
    }
    
    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        GameObject obj;
        if (m_pool.Count > 0)
        {
            obj = m_pool.Dequeue();
            obj.SetActive(true);
        }
        else
        {
            obj = GameObject.Instantiate(m_prefab);
            obj.transform.SetParent(m_poolRoot);
        }
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    public GameObject GetObject()
    {
        return GetObject(Vector3.zero, Quaternion.identity);
    }

    public void DestroyObject(GameObject go)
    {
        go.SetActive(false);
        m_pool.Enqueue(go);
    }

    public void ClearPool()
    {
        while (m_pool.Count > 0)
        {
            GameObject obj = m_pool.Dequeue();
            GameObject.Destroy(obj);
        }
    }
}
