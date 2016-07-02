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
    private GameObject[] m_muzzleFlashes;
    private AudioSource[] m_gunShotAudioSources;


    void Awake()
    {
        m_flyingControlScript = GetComponent<FlyingControl>();
        m_numSpawnPoints = m_bulletSpawnPoints.Length;
        m_muzzleFlashWait = new WaitForSeconds(m_muzzleFlashTime);
    }


    void Start()
    {
        m_muzzleFlashes = new GameObject[m_bulletSpawnPoints.Length];
        m_gunShotAudioSources = new AudioSource[m_bulletSpawnPoints.Length];

        for (int i = 0; i < m_bulletSpawnPoints.Length; i++)
        {
            var spawnPoint = m_bulletSpawnPoints[i];

            var muzzleFlash = spawnPoint.GetComponentInChildren<MeshRenderer>();

            if (muzzleFlash != null)
            {
                muzzleFlash.gameObject.SetActive(false);
                m_muzzleFlashes[i] = muzzleFlash.gameObject;
            }

            var gunShotAudio = spawnPoint.GetComponentInChildren<AudioSource>();

            if (gunShotAudio != null)
            {
                m_gunShotAudioSources[i] = gunShotAudio;
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

            var muzzleFlash = m_muzzleFlashes[m_spawnPointIndex];

            if (muzzleFlash != null)
            { 
                muzzleFlash.SetActive(true);

                StartCoroutine(TurnOffQuad(muzzleFlash));
            }

            var gunShotAudio = m_gunShotAudioSources[m_spawnPointIndex];

            if (gunShotAudio != null && gunShotAudio.clip != null)
            {
                gunShotAudio.PlayOneShot(gunShotAudio.clip);
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
