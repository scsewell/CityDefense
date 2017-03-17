using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton
{
    [SerializeField]
    private Bullet m_bulletPrefab;
    [SerializeField]
    private int m_bulletPoolSize = 100;


}
