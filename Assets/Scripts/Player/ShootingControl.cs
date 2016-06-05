using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FlyingControl))]
public class ShootingControl : MonoBehaviour
{
    [SerializeField] BulletFlight m_bullet;
    [SerializeField] float m_bulletCooldown = 0.2f;
    [SerializeField] float m_spawnDistanceInFront = 5f;
    [SerializeField] float m_spawnDistanceSideways = 2f;
    [SerializeField] float m_spawnDistanceUp = 0f;

    private float m_timeSinceBulletFired;
    private FlyingControl m_flyingControlScript;
    private bool m_rightSide;


    void Awake()
    {
        m_flyingControlScript = GetComponent<FlyingControl>();
    }


    void Update()
    {
        m_timeSinceBulletFired += Time.deltaTime;  
    }


    public void Shoot(float input)
    {
        if (input == 1 && m_timeSinceBulletFired > m_bulletCooldown)
        {
            //print("Instantiate bullet");
            m_timeSinceBulletFired = 0;
            var bullet = Instantiate(m_bullet);

            var sidewaysOffset = m_rightSide ? transform.right : -transform.right;

            bullet.transform.position = transform.position
                + transform.up * m_spawnDistanceUp
                + transform.forward * m_spawnDistanceInFront
                + sidewaysOffset * m_spawnDistanceSideways;

            bullet.transform.rotation = transform.rotation;

            bullet.SetInitialVelocity(m_flyingControlScript.ForwardVelocity);
            m_rightSide = !m_rightSide;
        }
    }
}
