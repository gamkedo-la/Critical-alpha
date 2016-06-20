using UnityEngine;
using System.Collections;

public class ShootingControl : MonoBehaviour
{
    [SerializeField] BulletFlight m_bullet;
    [SerializeField] Transform[] m_bulletSpawnPoints;
    [SerializeField] float m_bulletCooldown = 0.15f;
    [SerializeField] float m_muzzleFlashTime = 0.05f;

    private float m_timeSinceBulletFired;
    private FlyingControl m_flyingControlScript;
    private int m_spawnPointIndex;
    private int m_numSpawnPoints;
    private WaitForSeconds m_muzzleFlashWait;


    void Awake()
    {
        m_flyingControlScript = GetComponent<FlyingControl>();
        m_numSpawnPoints = m_bulletSpawnPoints.Length;
        m_muzzleFlashWait = new WaitForSeconds(m_muzzleFlashTime);
    }


    void Start()
    {
        for (int i = 0; i < m_bulletSpawnPoints.Length; i++)
        {
            var spawnPoint = m_bulletSpawnPoints[i];
            
            if (spawnPoint.childCount > 0)
            {
                var muzzleFlash = spawnPoint.GetChild(0);
                muzzleFlash.gameObject.SetActive(false);
            }              
        }
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

            if (spawnPoint.childCount > 0)
            {
                var muzzleFlash = spawnPoint.GetChild(0);
                muzzleFlash.gameObject.SetActive(true);

                StartCoroutine(TurnOffQuad(muzzleFlash.gameObject));
            }

            m_spawnPointIndex++;
            m_spawnPointIndex = m_spawnPointIndex % m_numSpawnPoints;   
        }
    }


    private IEnumerator TurnOffQuad(GameObject muzzleFlashObject)
    {
        yield return m_muzzleFlashWait;

        muzzleFlashObject.SetActive(false);
    }
}
