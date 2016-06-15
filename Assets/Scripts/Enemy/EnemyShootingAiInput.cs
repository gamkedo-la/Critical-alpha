using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ShootingControl))]
public class EnemyShootingAiInput : MonoBehaviour
{
    [SerializeField] float m_minDotProductForShooting = 0.5f;

    private ShootingControl m_shootingControlScript;
    private Transform m_player;


    void Awake()
    {
        m_shootingControlScript = GetComponent<ShootingControl>();
        var playerObject = GameObject.FindGameObjectWithTag(Tags.Player);

        if (playerObject != null)
            m_player = playerObject.transform;
    }


    void Update()
    {
        if (m_player == null)
            return;

        if (IsPlayerInfront())
            m_shootingControlScript.Shoot();
	}


    private bool IsPlayerInfront()
    {
        var direction = (m_player.position - transform.position).normalized;

        float dot = Vector3.Dot(direction, transform.forward);

        bool isInfront = dot >= m_minDotProductForShooting;

        return isInfront;
    }
}
