using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable 
{
	[SerializeField] int m_startingHealth = 100;
	[SerializeField] ParticleSystem m_explosion;
    [SerializeField] MeshFilter m_explosionMesh;
    [SerializeField] int m_damageCausedToOthers = 100;
    [SerializeField] bool m_allowDestroyedByGround;
    [SerializeField] bool m_allowDestroyedByWater;
    [SerializeField] Transform[] m_objectsToDetatchOnDeath;

	private int m_currentHealth;


	void Awake()
	{
		m_currentHealth = m_startingHealth;
	}


    public void Damage(int damage)
    {
        m_currentHealth -= damage;

        //print("Damaged by " + damage + ", current health = " + m_currentHealth);

        if (m_currentHealth <= 0)
            Dead();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Bullet))
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
        }

        //Dead();
    }


    private void Dead()
    {
        if (m_explosion != null)
        {
            var explosion = Instantiate(m_explosion);
            explosion.transform.position = transform.position;

            if (m_explosionMesh != null)
            {
                print(m_explosionMesh.transform.lossyScale);
                explosion.transform.localScale = m_explosionMesh.transform.lossyScale;
                var shape = explosion.shape;
                shape.shapeType = ParticleSystemShapeType.Mesh;
                shape.mesh = m_explosionMesh.mesh;
                explosion.transform.rotation = m_explosionMesh.transform.rotation;
            }

            float lifetime = explosion.startLifetime;
            Destroy(explosion.gameObject, lifetime);
        }

        for (int i = 0; i < m_objectsToDetatchOnDeath.Length; i++)
            m_objectsToDetatchOnDeath[i].parent = null;

        EventManager.TriggerEvent(TransformEventName.EnemyDead, transform);

        Destroy(gameObject);
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
