using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] int m_startingHealth = 300;

    private int m_currentHealth;
    private bool m_dead;


    void Awake()
    {
        m_currentHealth = m_startingHealth;
    }


    public void Damage(int damage)
    {
        m_currentHealth -= damage;

        if (m_currentHealth <= 0)
            PlayerDead("");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            PlayerDead("");
    }


    void OnTriggerStay(Collider other)
    {     
        if (m_dead || other.CompareTag(Tags.Bullet))
            return;

        //print(other.name);

        //if (other.CompareTag(Tags.Ground) || other.CompareTag(Tags.Water))
        PlayerDead(other.tag);
    }


    private void PlayerDead(string colliderTag)
    {
        //print(colliderTag);
        m_dead = true;
        EventManager.TriggerEvent(StringEventName.PlayerDead, colliderTag);
        Destroy(gameObject);
    }
}
