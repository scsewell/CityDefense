using UnityEngine;

public class Factory : Target
{
    [SerializeField]
    private float m_targetWidth = 1.25f;

    public override Vector3 GetTargetPos()
    {
        return transform.position + Vector3.right * Random.Range(-m_targetWidth, m_targetWidth);
    }
}
