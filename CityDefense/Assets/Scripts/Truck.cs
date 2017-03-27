using UnityEngine;

public class Truck : Target
{
    [SerializeField]
    private float m_maxSpeed = 0.25f;
    [SerializeField]
    private float m_acceleration = 0.125f;

    private TransformInterpolator m_interpolator;
    private float m_speed;

    protected override void OnAwake()
    {
        m_interpolator = GetComponent<TransformInterpolator>();

        m_health.OnDie += OnDie;

        Rigidbody body = gameObject.AddComponent<Rigidbody>();
        body.isKinematic = true;
    }

    private void OnDestroy()
    {
        m_health.OnDie -= OnDie;
    }

    private void OnEnable()
    {
        m_interpolator.ForgetPreviousValues();
        m_health.ResetHealth();
        if (m_healthbar != null)
        {
            m_healthbar.gameObject.SetActive(true);
            m_healthbar.Init();
        }
        m_speed = 0f;
        EnemyManager.Instance.AddTarget(this);
    }

    private void OnDisable()
    {
        if (m_healthbar != null)
        {
            m_healthbar.gameObject.SetActive(false);
        }
        EnemyManager.Instance.RemoveTarget(this);
    }

    private void OnDie()
    {
        Deactivate();
    }

    protected override void OnFixedUpdate()
    {
        m_speed = Mathf.MoveTowards(m_speed, m_maxSpeed, m_acceleration * Time.deltaTime);

        Vector3 disp = GameManager.Instance.City.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(disp, Vector3.up), Time.deltaTime);
        transform.position += transform.forward * m_speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Targets") && other.GetComponentInParent<City>() != null)
        {
            PlayerManager.Instance.Bullet1AmmoArrived();
            Deactivate();
        }
    }
}
