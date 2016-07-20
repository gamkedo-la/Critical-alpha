using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable 
{
	[SerializeField] int m_startingHealth = 100;
    [SerializeField] GameObject m_aliveModel;
    [SerializeField] GameObject m_deadModel;
	[SerializeField] ParticleSystem m_explosion;
    [SerializeField] ParticleSystem m_waterSplash;
    [SerializeField] GameObject m_fireAndSmoke;
    [SerializeField] MeshFilter m_explosionMesh;
    [SerializeField] int m_damageCausedToOthers = 100;
    [SerializeField] bool m_becomePhysicsObjectOnDeath;
    [SerializeField] bool m_allowDestroyedByGround;
    [SerializeField] bool m_allowDestroyedByWater;
    [SerializeField] Transform[] m_objectsToDetatchOnDeath;
    [SerializeField] float m_transformJustDamagedResetTime = 0.1f;

	private int m_currentHealth;
    private bool m_dead;
    private Transform m_transformJustDamaged;
    private AudioClipBucket m_audioClipBucket;
    private Rigidbody m_rigidBody;
    private bool m_inWater;
    private bool m_crashedOnGround;
    private GameObject m_activeFireAndSmoke;


    void Awake()
	{
		m_currentHealth = m_startingHealth;
        m_audioClipBucket = GetComponent<AudioClipBucket>();
        m_rigidBody = GetComponent<Rigidbody>();

        if (m_aliveModel != null && m_deadModel != null)
        {
            m_aliveModel.SetActive(true);
            m_deadModel.SetActive(false);
        }
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


    public bool IsDead { get { return m_dead; } }


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

        if (!m_dead)
            return;

        if (other.CompareTag(Tags.Water) && m_waterSplash != null && !m_inWater)
        {
            InstantiateWaterSplash();
            EnterWater();
        }
    }


    void OnCollisionEnter(Collision col)
    {
        if (m_inWater || m_crashedOnGround || (col.gameObject.CompareTag(Tags.Ground) && !m_allowDestroyedByGround))
            return;

        print(name + " crashed into ground");
        m_crashedOnGround = true;

        if (col.gameObject.CompareTag(Tags.Ground) && m_explosion != null)
        {
            InstantiateExplosion();
            m_rigidBody.velocity *= 0.1f;
            m_rigidBody.drag = 1f;
        }
    }


    private void EnterWater()
    {
        m_inWater = true;
        m_rigidBody.drag = 1f;

        var particleSystems = m_activeFireAndSmoke.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < particleSystems.Length; i++)
        {
            //print("Stopping particle system " + particleSystems[i].name);
            particleSystems[i].Stop();
        }

        var light = m_activeFireAndSmoke.GetComponentInChildren<Light>();

        if (light != null)
            light.enabled = false;
    }


    private IEnumerator ResetTransformJustDamaged()
    {
        yield return new WaitForSeconds(m_transformJustDamagedResetTime);

        m_transformJustDamaged = null;
    }


    private void InstantiateWaterSplash()
    {
        var waterSplash = Instantiate(m_waterSplash);
        waterSplash.transform.position = transform.position;

        float lifetime = waterSplash.startLifetime; // Mathf.Max(clipLength, explosion.startLifetime);
        Destroy(waterSplash.gameObject, lifetime * 1.5f);
    }


    private void InstantiateExplosion()
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


    private void Dead()
    {
        if (m_dead)
            return;

        m_dead = true;

        if (m_explosion != null)
            InstantiateExplosion();

        if (m_fireAndSmoke != null)
        {
            m_activeFireAndSmoke = Instantiate(m_fireAndSmoke);
            m_activeFireAndSmoke.transform.position = transform.position;
            m_activeFireAndSmoke.transform.parent = transform;
        }

        for (int i = 0; i < m_objectsToDetatchOnDeath.Length; i++)
            m_objectsToDetatchOnDeath[i].parent = null;

        EventManager.TriggerEvent(TransformEventName.EnemyDead, transform);

        if (m_aliveModel != null && m_deadModel != null)
        {
            m_aliveModel.SetActive(false);
            m_deadModel.SetActive(true);
        }
        else if (!m_becomePhysicsObjectOnDeath)
            Destroy(gameObject, 0.05f);
        
        if (m_becomePhysicsObjectOnDeath && m_rigidBody != null)
            BecomePhysicsObject();
    }


    private void BecomePhysicsObject()
    {
        m_rigidBody.isKinematic = false;
        m_rigidBody.useGravity = true;

        var flightControlScript = GetComponent<FlyingControl>();
        var flightAiInputScript = GetComponent<EnemyAircraftAiInput>();
        var shootingControlScript = GetComponent<ShootingControl>();
        var shootingAiInputScript = GetComponent<EnemyShootingAiInput>();

        if (flightControlScript != null)
        {
            m_rigidBody.velocity = flightControlScript.ForwardVelocityInWorld;
            flightControlScript.enabled = false;
        }

        if (flightAiInputScript != null)
            flightAiInputScript.enabled = false;

        if (shootingControlScript != null)
            shootingControlScript.enabled = false;

        if (shootingAiInputScript != null)
            shootingAiInputScript.enabled = false;

        m_rigidBody.AddTorque(transform.forward * Random.Range(-10f, 10f), ForceMode.Impulse);

        StartCoroutine(WaitForSleep());
    }


    private IEnumerator WaitForSleep()
    {
        while (!m_rigidBody.IsSleeping() && m_rigidBody.velocity.sqrMagnitude > 0.1f)
            yield return null;

        m_rigidBody.isKinematic = true;
        m_rigidBody.useGravity = false;
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
