using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField]
    private float m_speed = 4.0f;
    [SerializeField]
    private float m_lifespan = 10.0f;

    private TransformInterpolator m_interpolator;
    private TrailRenderer m_trail;
    private float m_creationTime;

    private void Awake()
    {
        m_interpolator = GetComponent<TransformInterpolator>();
        m_trail = GetComponent<TrailRenderer>();
    }

    public override void Init(Player owner)
    {
        base.Init(owner);
        m_interpolator.ForgetPreviousValues();
        m_trail.Clear();
        m_creationTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (Time.time - m_creationTime > m_lifespan)
        {
            Deactivate();
        }
        transform.position += transform.forward * m_speed * Time.deltaTime;
    }
}
