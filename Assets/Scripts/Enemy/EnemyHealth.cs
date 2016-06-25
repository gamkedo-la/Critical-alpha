using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable 
{
	[SerializeField] int m_startingHealth = 100;
	[SerializeField] ParticleSystem m_explosion;
    [SerializeField] MeshFilter m_explosionMesh;

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

			Destroy(gameObject);
		}
	}


    public int CurrentHealth
    {
        get { return m_currentHealth; }
    }
}
