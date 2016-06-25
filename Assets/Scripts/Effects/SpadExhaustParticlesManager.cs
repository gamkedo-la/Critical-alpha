using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class SpadExhaustParticlesManager : MonoBehaviour
{
    [SerializeField] float m_minEmissionRateFraction = 0.2f;

    private ParticleSystem m_particleSystem;
    private ParticleSystem.EmissionModule m_emission;
    private FlyingControl m_flyingControlScript;
    private float m_maxRate;
    private float m_maxSpeed;


	void Awake()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
        m_emission = m_particleSystem.emission;
        m_flyingControlScript = transform.root.gameObject.GetComponent<FlyingControl>();
        m_maxRate = m_emission.rate.constantMax;
        m_maxSpeed = m_flyingControlScript.MaxForwardSpeed;
    }


    void Update()
    {
        if (m_flyingControlScript == null)
            return;

        float minRate = m_maxRate * m_minEmissionRateFraction;
        float speed = m_flyingControlScript.ForwardSpeed;

        float rateValue = Mathf.Lerp(minRate, m_maxRate, speed / m_maxSpeed);

        var rate = m_emission.rate;
        rate.constantMin = rateValue;
        rate.constantMax = rateValue;

        m_emission.rate = rate;
    }
}
