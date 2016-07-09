using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerDeadCameraController : MonoBehaviour
{
    [SerializeField] float m_deathCameraPanSpeed = 20f;
    [SerializeField] ParticleSystem m_explosionParticles;
    [SerializeField] ParticleSystem m_waterSplashParticles;
    [SerializeField] ParticleSystem m_fireParticles;

    private bool m_dead;
    private bool m_missionSuccessful;


    void Update()
    {
        if (m_dead || m_missionSuccessful)
        {
            transform.Rotate(Vector3.up, m_deathCameraPanSpeed * Time.unscaledDeltaTime, Space.World);

            //if (Input.GetKeyDown(KeyCode.R))
            //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    private void PlayerDead(string colliderTag)
    {
        m_dead = true;
        transform.rotation = Quaternion.identity;

        if (colliderTag == Tags.Water && m_waterSplashParticles != null)
        {
            var waterSplash = (ParticleSystem) Instantiate(m_waterSplashParticles, transform.position, m_waterSplashParticles.transform.rotation);
            Destroy(waterSplash.gameObject, waterSplash.startLifetime);
        }

        if (m_explosionParticles != null)
        {
            var explosion = (ParticleSystem) Instantiate(m_explosionParticles, transform.position, Quaternion.identity);
            Destroy(explosion.gameObject, explosion.startLifetime);
        }

        if (m_fireParticles != null && colliderTag != Tags.Water)
            Instantiate(m_fireParticles, transform.position, Quaternion.identity);

        DetatchCamera();
        
        EventManager.TriggerEvent(StandardEventName.MissionFailed);
        print("Mission failed");
    }


    private void MissionSuccessful()
    {
        transform.rotation = Quaternion.identity;
        Time.timeScale = 0;
        m_missionSuccessful = true;
        DetatchCamera();
    }


    private void DetatchCamera()
    {
        var cameraPosition = transform.GetChild(0);

        Camera.main.transform.parent = cameraPosition;
        Camera.main.transform.position = cameraPosition.position;
        Camera.main.transform.rotation = cameraPosition.rotation;

        transform.parent = null;

        EventManager.TriggerEvent(BooleanEventName.ActivateHud, false);
        EventManager.TriggerEvent(BooleanEventName.ActivateRadar, false);
        EventManager.TriggerEvent(BooleanEventName.ActivateTargetSystem, false);
        EventManager.TriggerEvent(BooleanEventName.ActivateHealthMeter, false);
    }


    void OnEnable()
    {
        EventManager.StartListening(StringEventName.PlayerDead, PlayerDead);
        EventManager.StartListening(StandardEventName.ActivateCameraPan, MissionSuccessful);
    }


    void OnDisable()
    {
        EventManager.StopListening(StringEventName.PlayerDead, PlayerDead);
        EventManager.StopListening(StandardEventName.ActivateCameraPan, MissionSuccessful);
    }
}
