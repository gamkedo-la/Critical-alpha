using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ExplosionAudioManager : MonoBehaviour
{
    [SerializeField] Vector2 m_pitchMinMax = new Vector2(0.8f, 1.2f);

    private AudioSource m_audioSource;


    void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.pitch = Random.Range(m_pitchMinMax.x, m_pitchMinMax.y);
    }


    public void SetClip(AudioClip clip)
    {
        m_audioSource.clip = clip;
    }
}
