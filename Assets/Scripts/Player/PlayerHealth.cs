using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public static bool PlayerDead;

    [SerializeField] bool m_invulnerable = false;
    [SerializeField] int m_startingHealth = 300;
    [SerializeField] int m_damageCausedToOthers = 100;
    [SerializeField] Transform[] m_objectsToDetatchOnDeath;

    private int m_currentHealth;
    private bool m_dead;


    void Awake()
    {
        m_currentHealth = m_startingHealth;
    }


    public void Damage(int damage)
    {
        if (m_invulnerable || m_dead)
            return;

        m_currentHealth -= damage;

        if (m_currentHealth <= 0)
            Dead("");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            Dead("");
    }


    void OnTriggerEnter(Collider other)
    {     
        if (m_invulnerable || m_dead || other.CompareTag(Tags.Bullet))
            return;

        var otherDamageScript = other.gameObject.GetComponentInParent<IDamageable>();

        if (otherDamageScript != null)
        {
            print(string.Format("{0} causes {1} damage to {2}", name, m_damageCausedToOthers, other.name));
            otherDamageScript.Damage(m_damageCausedToOthers);
        }

        if (other.CompareTag(Tags.Ground) || other.CompareTag(Tags.Water))
            Dead(other.tag);
    }


    private void Dead(string colliderTag)
    {
        //print(colliderTag);
        m_dead = true;
        
        EventManager.TriggerEvent(StringEventName.PlayerDead, colliderTag);

        for (int i = 0; i < m_objectsToDetatchOnDeath.Length; i++)
        {
            var objectToDetatch = m_objectsToDetatchOnDeath[i];
            if (objectToDetatch != null)
                objectToDetatch.parent = null;
        }

        m_currentHealth = 0;

        PlayerDead = true;

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
