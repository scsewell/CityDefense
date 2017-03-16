﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int m_score;
    [SerializeField]
    private float m_damage;

    private Health m_health;
    
    protected void Awake()
    {
        m_health = GetComponent<Health>();
        m_health.OnDie += OnDie;

        Rigidbody body = gameObject.AddComponent<Rigidbody>();
        body.isKinematic = true;
    }

    private void OnDie()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Targets"))
        {
            Health health = other.gameObject.GetComponent<Health>();
            health.ApplyDamage(m_damage);
            Destroy(gameObject);
        }
        else
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            m_health.ApplyDamage(projectile.GetDamage());
            projectile.Destroy();
        }
    }
}