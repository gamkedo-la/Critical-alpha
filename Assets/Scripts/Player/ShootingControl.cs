using UnityEngine;
using System.Collections;

public class ShootingControl : MonoBehaviour
{
    [SerializeField] BulletFlight m_bullet;
    [SerializeField] Transform[] m_bulletSpawnPoints;
    [SerializeField] float m_bulletCooldown = 0.15f;

    private float m_timeSinceBulletFired;
    private FlyingControl m_flyingControlScript;
    private int m_spawnPointIndex;
    private int m_numSpawnPoints;


    void Awake()
    {
        m_flyingControlScript = GetComponent<FlyingControl>();
        m_numSpawnPoints = m_bulletSpawnPoints.Length;
    }


    void Update()
    {
        m_timeSinceBulletFired += Time.deltaTime;  
    }


    public void Shoot()
    {
        if (m_timeSinceBulletFired > m_bulletCooldown)
        {
            //print("Instantiate bullet");
            m_timeSinceBulletFired = 0;
            var bullet = Instantiate(m_bullet);
            bullet.transform.parent = transform;

            var spawnPoint = m_bulletSpawnPoints[m_spawnPointIndex];

            bullet.transform.position = spawnPoint.position;

            bullet.transform.rotation = spawnPoint.rotation;

            if (m_flyingControlScript != null)
                bullet.SetInitialVelocity(m_flyingControlScript.ForwardVelocity);
            else
                bullet.SetInitialVelocity(Vector3.zero);

            m_spawnPointIndex++;
            m_spawnPointIndex = m_spawnPointIndex % m_numSpawnPoints;
        }
    }
}
