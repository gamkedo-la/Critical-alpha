using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(TrailRenderer))]
public class BulletFlight : MonoBehaviour
{
	[SerializeField] int m_bulletDamage = 10;
    [SerializeField] float m_bulletLifeTime = 4f;
    [SerializeField] float m_bulletSpeed = 100f;
    [SerializeField] ParticleSystem m_explosion;
    [SerializeField] ParticleSystem m_splash;
    [SerializeField] AudioClipByTag[] m_audioClips;
    
    private Vector3 m_velocity;
    private bool m_bulletImpacted;
    private Transform m_originalParent;


    void Start()
    {
        Destroy(gameObject, m_bulletLifeTime);
        m_originalParent = transform.root;
        transform.parent = null;
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

        if ((other.transform == m_originalParent)
            || (other.transform.root == m_originalParent))
            return;

        //print("Bullet impact with " + other.name);

        ParticleSystem particles;

        if (other.CompareTag(Tags.Water))
            particles = m_splash;
        else
            particles = m_explosion;

        m_bulletImpacted = true;  

		IDamageable damageScript = other.gameObject.GetComponentInParent<IDamageable>();

		if (damageScript != null)
			damageScript.Damage(m_bulletDamage);

        var explosion = Instantiate(particles);
        explosion.transform.position = transform.position;
        float lifetime = explosion.startLifetime;

        var explosionAudio = explosion.gameObject.GetComponent<ExplosionAudioManager>();

        if (explosionAudio != null)
        {
            for (int i = 0; i < m_audioClips.Length; i++)
            {
                var clip = m_audioClips[i];

                if (other.CompareTag(clip.tag))
                    explosionAudio.SetClip(clip.audioClip);
            }
        }

        Destroy(explosion.gameObject, lifetime);
        Destroy(gameObject);
    }
}
