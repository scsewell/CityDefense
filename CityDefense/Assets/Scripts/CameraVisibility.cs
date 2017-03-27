using UnityEngine;

public class CameraVisibility : Singleton<CameraVisibility>
{
    private Camera m_cam;
    private Plane[] m_planes;
    
    private void Start ()
    {
        m_cam = GetComponent<Camera>();
        m_planes = GeometryUtility.CalculateFrustumPlanes(m_cam);
    }
	
	public bool IsVisible(Collider collider)
    {
        return GeometryUtility.TestPlanesAABB(m_planes, collider.bounds);
    }
}