using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public static bool PlayerDead;

    [SerializeField] bool m_invulnerable = false;
    [SerializeField] int m_startingHealth = 300;
    [SerializeField] int m_damageCausedToOthers = 100;
    [SerializeField] Transform[] m_objectsToDetatchOnDeath;
    [SerializeField] float m_transformJustDamagedResetTime = 0.1f;

    private int m_currentHealth;
    private bool m_dead;
    private Transform m_transformJustDamaged;


    void Awake()
    {
        m_currentHealth = m_startingHealth;
        PlayerDead = false;
    }


    public void Damage(int damage)
    {
        if (m_invulnerable || m_dead)
            return;

        m_currentHealth -= damage;

        //print(string.Format("{0} damaged by {1}, current health = {2}", name, damage, m_currentHealth));

        if (m_currentHealth <= 0)
            Dead("");
    }


    public bool IsDead { get { return m_dead; } }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            Dead("");
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Bullet)
            || (m_transformJustDamaged != null && m_transformJustDamaged == other.transform))
            return;

        var otherDamageScript = other.gameObject.GetComponentInParent<IDamageable>();

        if (otherDamageScript != null)
        {
            //print(string.Format("{0} causes {1} damage to {2}", name, m_damageCausedToOthers, other.name));
            otherDamageScript.Damage(m_damageCausedToOthers);
            m_transformJustDamaged = other.transform;
            StartCoroutine(ResetTransformJustDamaged());
        } 

        if (!m_invulnerable && (other.CompareTag(Tags.Ground) || other.CompareTag(Tags.Water)))
            Dead(other.tag);
    }


    private IEnumerator ResetTransformJustDamaged()
    {
        yield return new WaitForSeconds(m_transformJustDamagedResetTime);

        m_transformJustDamaged = null;
    }


    private void Dead(string colliderTag)
    {
        if (m_dead)
            return;

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
