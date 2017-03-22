using UnityEngine;

public class Rocket : Enemy
{
    [SerializeField]
    private Transform m_mesh;
    [SerializeField]
    private Transform m_explosionPos;

    [SerializeField]
    private float m_speed = 5.0f;
    [SerializeField]
    private float m_turnSpeed = 20.0f;
    [SerializeField]
    private float m_startAngleVariance = 10.0f;
    [SerializeField]
    private float m_maxRoll = 5.0f;

    private TransformInterpolator m_interpolator;
    private Transform m_target;
    private Vector3 m_targetPos;
    private float m_rollSpeed;
    private float m_rotation;

    protected override void Awake()
    {
        base.Awake();
        m_interpolator = GetComponent<TransformInterpolator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        m_interpolator.ForgetPreviousValues();
        m_rotation = getTargetRot() + Random.Range(-m_startAngleVariance, m_startAngleVariance);
        m_rollSpeed = Random.Range(-m_maxRoll, m_maxRoll);
        m_mesh.localEulerAngles = new Vector3(m_mesh.localEulerAngles.x, Random.Range(0f, 360f), m_mesh.localEulerAngles.z);
    }

    protected override void OnDestroyed()
    {
        PoolManager.GetExplosion1(m_explosionPos.position, Quaternion.identity);
    }

    protected override void OnUpdate()
    {
        m_mesh.localEulerAngles = m_mesh.localEulerAngles + Vector3.up * m_rollSpeed * Time.deltaTime;
    }

    protected override void OnFixedUpdate()
    {
        m_rotation = Mathf.Lerp(m_rotation, getTargetRot(), Time.deltaTime * m_turnSpeed);

        transform.rotation = Quaternion.AngleAxis(m_rotation, Vector3.forward);
        transform.position += -transform.up * m_speed * Time.deltaTime;
    }

    private float getTargetRot()
    {
        Vector3 targetPos = m_target != null ? m_target.position : m_targetPos;
        Vector3 disp = targetPos - transform.position;
        return ((360 - (Mathf.Atan2(disp.x, disp.y) * Mathf.Rad2Deg)) % 360) - 180;
    }

    public void SetTarget(Transform t)
    {
        m_target = t;
    }

    public void SetTarget(Vector3 t)
    {
        m_targetPos = t;
    }
}
