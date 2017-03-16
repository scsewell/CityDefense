using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 4.0f;
    [SerializeField]
    private float m_lifespan = 10.0f;

    private Player m_player;
    private float m_creationTime;

	private void Start()
    {
        m_creationTime = Time.time;
	}

    private void Init(Player player)
    {
        m_player = player;
    }

    private void FixedUpdate()
    {
        if (Time.time - m_creationTime > m_lifespan)
        {
            Destroy(gameObject);
        }
        transform.position += transform.forward * m_speed * Time.deltaTime;
    }
}
