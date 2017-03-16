using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private Transform m_turret;
    [SerializeField]
    private Transform m_barrel;
    [SerializeField]
    private Transform m_barrelMesh;
    [SerializeField]
    private Transform m_barrelEnd;

    [SerializeField]
    private Bullet m_bulletPrefab;
    [SerializeField]
    private int m_bulletPoolSize = 100;
    [SerializeField]
    private float m_fireRate = 1.0f;
    [SerializeField]
    private float m_flashBrighness = 2.0f;
    [SerializeField]
    private float m_recoilAmount = 0.065f;
    [SerializeField]
    private ParticleSystem m_smokePrefab;
    private static ObjectPool m_bulletPool;

    private Player m_owner;
    private Light m_light;
    private Interpolator<float> m_brightness;
    private float m_lastFireTime;

    private void Awake()
    {
        if (m_bulletPool == null)
        {
            m_bulletPool = new ObjectPool(m_bulletPrefab.gameObject, m_bulletPoolSize);
        }

        m_light = m_barrelEnd.GetComponent<Light>();
        m_light.intensity = 0;
        InterpolatedFloat brightness = new InterpolatedFloat(() => (m_light.intensity), (val) => { m_light.intensity = val; });
        m_brightness = gameObject.AddComponent<FloatInterpolator>().Initialize(brightness);
    }

    private void Init(Player owner)
    {
        m_owner = owner;
    }

    public void StateUpdate(Vector3 targetPos)
    {
        Vector3 disp = targetPos - m_barrel.position;
        m_turret.localRotation = Quaternion.AngleAxis(Mathf.Atan2(disp.y, disp.x * 5) * Mathf.Rad2Deg - 90, Vector3.forward);
        m_barrel.localRotation = Quaternion.AngleAxis(Mathf.Atan2(disp.y, Mathf.Abs(disp.x)) * Mathf.Rad2Deg, Vector3.right);

        m_light.intensity /= 2.5f;
        m_barrelMesh.transform.position = Vector3.Lerp(m_barrelMesh.transform.position, m_barrel.position, Time.deltaTime * 8.0f);
    }

    public void FireBullet(Vector3 targetPos)
    {
        if (Time.time - m_lastFireTime > (1 / m_fireRate))
        {
            Vector3 pos = Vector3.ProjectOnPlane(m_barrelEnd.position, Vector3.forward);
            Vector3 disp = targetPos - pos;
            m_bulletPool.GetObject(pos, Quaternion.LookRotation(disp, Vector3.up)).GetComponent<Projectile>().Init(m_owner, m_bulletPool);

            Instantiate(m_smokePrefab, pos, Quaternion.identity);

            m_brightness.ForgetPreviousValues();
            m_light.intensity = m_flashBrighness;

            m_barrelMesh.transform.position = m_barrel.position - m_barrel.up * m_recoilAmount;

            m_lastFireTime = Time.time;
        }
    }
}
