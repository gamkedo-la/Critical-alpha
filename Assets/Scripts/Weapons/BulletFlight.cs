using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(TrailRenderer))]
public class BulletFlight : MonoBehaviour
{
    [SerializeField] float m_bulletLifeTime = 4f;
    [SerializeField] float m_bulletSpeed = 100f;
    [SerializeField] ParticleSystem m_explosion;
    [SerializeField] ParticleSystem m_splash;
    
    private Vector3 m_velocity;
    private bool m_bulletImpacted;
    private TrailRenderer m_trailRenderer;
    //private SphereCollider m_collider;
    //private float m_colliderRadius;


    void Start()
    {
        m_trailRenderer = GetComponent<TrailRenderer>();

        Destroy(gameObject, m_bulletLifeTime);
 
        //m_velocity = Vector3.down * m_bulletSpeed;
        //m_collider = GetComponent<SphereCollider>();
        //m_colliderRadius = m_collider.radius * 0.5f;
    }


	void Update()
    {
        transform.Translate(m_velocity * Time.deltaTime);
	}


    public void SetInitialVelocity(Vector3 velocity)
    {
        m_velocity = velocity + Vector3.forward * m_bulletSpeed;
    }


    void OnTriggerStay(Collider other)
    {
        if (m_bulletImpacted)
            return;

        //print("Bullet impact");

        ParticleSystem particles;

        if (other.CompareTag("Water"))
            particles = m_splash;
        else
            particles = m_explosion;

        m_bulletImpacted = true;    
        var explosion = Instantiate(particles);
        explosion.transform.position = transform.position;
        float lifetime = explosion.startLifetime;
        Destroy(explosion.gameObject, lifetime);
        Destroy(gameObject);
    }
}
