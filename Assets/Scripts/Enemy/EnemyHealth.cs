using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable 
{
	[SerializeField] int m_startingHealth = 100;
	[SerializeField] ParticleSystem m_explosion;


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
			var explosion = Instantiate(m_explosion);
			explosion.transform.position = transform.position;
			float lifetime = explosion.startLifetime;
			Destroy(explosion.gameObject, lifetime);
			Destroy(gameObject);
		}
	}
}
