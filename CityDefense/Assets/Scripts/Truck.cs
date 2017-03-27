using UnityEngine;

public class Truck : Target
{
    [SerializeField]
    private float m_maxSpeed = 0.25f;
    [SerializeField]
    private float m_acceleration = 0.125f;

    private TransformInterpolator m_interpolator;
    private float m_speed = 0f;

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
        m_health.ResetHealth();
        m_healthbar.Init();
        m_interpolator.ForgetPreviousValues();
    }

    private void OnDisable()
    {
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
