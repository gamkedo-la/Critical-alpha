using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Collections;

public class GameController : MonoBehaviour
{
    [SerializeField] AudioMixerSnapshot m_sfxFullVolume;
    [SerializeField] AudioMixerSnapshot m_sfxMuted;

    private float m_sfxStartLevel;


    void Awake()
    {
        OnUnpause();
    }


    public void LoadLevel(string levelToLoad)
    {
        LastMenuUsed.LastMenuUsedName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(levelToLoad);
    }


    public void QuitGame()
    {
        Application.Quit();
    }


    private void OnPause()
    {
        Time.timeScale = 0;
        //print("Pause");
        if (m_sfxMuted != null)
            m_sfxMuted.TransitionTo(0.01f);
    }


    private void OnUnpause()
    {
        Time.timeScale = 1;
        //print("Unpause");
        if (m_sfxFullVolume != null)
            m_sfxFullVolume.TransitionTo(0.01f);
    }


    void OnEnable()
    {
        EventManager.StartListening(StandardEventName.Pause, OnPause);
        EventManager.StartListening(StandardEventName.Unpause, OnUnpause);
    }


    void OnDisable()
    {
        EventManager.StopListening(StandardEventName.Pause, OnPause);
        EventManager.StopListening(StandardEventName.Unpause, OnUnpause);
    }
}

