using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField]
    private float m_speed = 4.0f;
    [SerializeField]
    private float m_lifespan = 10.0f;

    private TransformInterpolator m_interpolator;
    private TrailRenderer m_trail;
    private Player m_player;
    private float m_creationTime;

    private void Awake()
    {
        m_interpolator = GetComponent<TransformInterpolator>();
        m_trail = GetComponent<TrailRenderer>();
    }

    public void Init(Player owner)
    {
        m_interpolator.ForgetPreviousValues();
        m_trail.Clear();
        m_creationTime = Time.time;
        m_player = owner;
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
