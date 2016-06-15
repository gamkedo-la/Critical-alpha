using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(PlaceableObjectGround))]
public class RigidbodyManager : MonoBehaviour
{
    [SerializeField] float m_distanceThreshold = 350f;

    private Rigidbody m_rigidbody;
    private Transform m_camera;

    private float m_distSq; 

    
    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_camera = Camera.main.transform;
        m_distSq = m_distanceThreshold * m_distanceThreshold;
    }


    void Start()
    {
        m_rigidbody.isKinematic = true;
    }


	void Update()
    {
        var pos2 = new Vector2(transform.position.x, transform.position.z);
        var cameraPos2 = new Vector2(m_camera.position.x, m_camera.position.z);

        float distSq = (pos2 - cameraPos2).sqrMagnitude;

        m_rigidbody.isKinematic = distSq > m_distSq;
	}
}
