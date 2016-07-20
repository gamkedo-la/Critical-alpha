using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [SerializeField] Vector2 m_intensityMultiplierMinMax = new Vector2(0.8f, 1.2f);
    [SerializeField] float m_flickerFrequency = 1f;

    private Light m_light;
    private float m_intensity;
    private float m_perlinX;

    
    void Awake()
    {
        m_light = GetComponent<Light>();
        m_intensity = m_light.intensity;
        m_perlinX = Random.Range(-100f, 100f);
    }
	

	void Update()
    {
        float noise = Mathf.PerlinNoise(m_perlinX, Time.time * m_flickerFrequency);
        m_light.intensity = m_intensity * Mathf.Lerp(m_intensityMultiplierMinMax.x, m_intensityMultiplierMinMax.y, noise);
	}
}
