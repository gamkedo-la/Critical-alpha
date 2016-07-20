using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable 
{
	[SerializeField] int m_startingHealth = 100;
    [SerializeField] GameObject m_aliveModel;
    [SerializeField] GameObject m_deadModel;
	[SerializeField] ParticleSystem m_explosion;
    [SerializeField] GameObject m_fireAndSmoke;
    [SerializeField] MeshFilter m_explosionMesh;
    [SerializeField] int m_damageCausedToOthers = 100;
    [SerializeField] bool m_allowDestroyedByGround;
    [SerializeField] bool m_allowDestroyedByWater;
    [SerializeField] Transform[] m_objectsToDetatchOnDeath;
    [SerializeField] float m_transformJustDamagedResetTime = 0.1f;

	private int m_currentHealth;
    private bool m_dead;
    private Transform m_transformJustDamaged;
    private AudioClipBucket m_audioClipBucket;


    void Awake()
	{
		m_currentHealth = m_startingHealth;
        m_audioClipBucket = GetComponent<AudioClipBucket>();
	}


    public void Damage(int damage)
    {
        if (m_dead)
            return;

        m_currentHealth -= damage;

        print(string.Format("{0} damaged by {1}, current health = {2}", name, damage, m_currentHealth));

        if (m_currentHealth <= 0)
            Dead();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Bullet)
            || Time.time < 0.1f     // To make sure no collisions happen during the placement algorithm
            || (m_transformJustDamaged != null && m_transformJustDamaged == other.transform))
            return;

        if (other.CompareTag(Tags.Water) && !m_allowDestroyedByWater)
            return;

        if (other.CompareTag(Tags.Ground) && !m_allowDestroyedByGround)
            return;

        var otherDamageScript = other.gameObject.GetComponentInParent<IDamageable>();

        if (otherDamageScript != null)
        {
            print(string.Format("{0} causes {1} damage to {2}", name, m_damageCausedToOthers, other.name));
            otherDamageScript.Damage(m_damageCausedToOthers);
            m_transformJustDamaged = other.transform;
            StartCoroutine(ResetTransformJustDamaged());
        }
        else
            Dead();
    }


    private IEnumerator ResetTransformJustDamaged()
    {
        yield return new WaitForSeconds(m_transformJustDamagedResetTime);

        m_transformJustDamaged = null;
    }


    private void Dead()
    {
        if (m_dead)
            return;

        m_dead = true;

        if (m_explosion != null)
        {
            var explosion = Instantiate(m_explosion);
            explosion.transform.position = transform.position;

            if (m_explosionMesh != null)
            {
                //print(m_explosionMesh.transform.lossyScale);
                explosion.transform.localScale = m_explosionMesh.transform.lossyScale;
                var shape = explosion.shape;
                shape.shapeType = ParticleSystemShapeType.Mesh;
                shape.mesh = m_explosionMesh.mesh;
                explosion.transform.rotation = m_explosionMesh.transform.rotation;
            }

            var explosionAudio = explosion.gameObject.GetComponent<ExplosionAudioManager>();
            float clipLength = 0;

            if (explosionAudio != null)
            {
                clipLength = explosionAudio.ClipLength;

                if (m_audioClipBucket != null)
                    explosionAudio.SetClips(m_audioClipBucket.explosionAudioClips);
            }

            float lifetime = Mathf.Max(clipLength, explosion.startLifetime);
            Destroy(explosion.gameObject, lifetime * 1.5f);
        }

        if (m_fireAndSmoke != null)
        {
            var fire = Instantiate(m_fireAndSmoke);
            fire.transform.position = transform.position;
        }

        for (int i = 0; i < m_objectsToDetatchOnDeath.Length; i++)
            m_objectsToDetatchOnDeath[i].parent = null;

        EventManager.TriggerEvent(TransformEventName.EnemyDead, transform);

        if (m_aliveModel != null && m_deadModel != null)
        {
            m_aliveModel.SetActive(false);
            m_deadModel.SetActive(true);
        }
        else
            Destroy(gameObject, 0.05f);
    }


    public int CurrentHealth
    {
        get { return m_currentHealth; }
    }


    public int StartingHealth
    {
        get { return m_startingHealth; }
    }
}
