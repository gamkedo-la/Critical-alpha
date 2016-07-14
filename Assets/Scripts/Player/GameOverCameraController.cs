﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverCameraController : MonoBehaviour
{
    [SerializeField] float m_deathCameraPanSpeed = 20f;
    [SerializeField] ParticleSystem m_explosionParticles;
    [SerializeField] ParticleSystem m_waterSplashParticles;
    [SerializeField] ParticleSystem m_fireParticles;

    private bool m_dead;
    private bool m_missionSuccessful;
    private AudioClipBucket m_audioClipBucket;


    void Awake()
    {
        m_audioClipBucket = GetComponentInParent<AudioClipBucket>();
    }


    void Update()
    {
        if (m_dead || m_missionSuccessful)
        {
            transform.Rotate(Vector3.up, m_deathCameraPanSpeed * Time.unscaledDeltaTime, Space.World);
        }
    }


    private void PlayerDead(string colliderTag)
    {
        m_dead = true;
        transform.rotation = Quaternion.identity;

        if (colliderTag == Tags.Water && m_waterSplashParticles != null)
        {
            var waterSplash = (ParticleSystem) Instantiate(m_waterSplashParticles, transform.position, m_waterSplashParticles.transform.rotation);

            var waterSplashAudio = waterSplash.gameObject.GetComponent<ExplosionAudioManager>();
            float clipLength = 0;

            if (waterSplashAudio != null)
            {
                clipLength = waterSplashAudio.ClipLength;
            }

            float lifetime = Mathf.Max(clipLength, waterSplash.startLifetime);
            Destroy(waterSplash.gameObject, lifetime * 1.5f);
        }

        if (m_explosionParticles != null)
        {
            var explosion = (ParticleSystem) Instantiate(m_explosionParticles, transform.position, Quaternion.identity);

            var explosionAudio = explosion.gameObject.GetComponent<ExplosionAudioManager>();
            float clipLength = 0;

            if (explosionAudio != null)
            {
                clipLength = explosionAudio.ClipLength;

                if (m_audioClipBucket != null)
                    explosionAudio.SetClips(m_audioClipBucket.explosionAudioClips);             
            }

            float lifetime = Mathf.Max(clipLength, explosion.startLifetime);
            Destroy(explosion.gameObject, lifetime * 1.5f);
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
