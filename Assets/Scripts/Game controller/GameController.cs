using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Collections;

public class GameController : MonoBehaviour
{
    [SerializeField] AudioMixerSnapshot m_sfxFullVolume;
    [SerializeField] AudioMixerSnapshot m_sfxMuted;
    [SerializeField] AudioMixerSnapshot m_musicFullVolume;
    [SerializeField] AudioMixerSnapshot m_musicMuted;

    [SerializeField] float m_musicFadeTime = 2f;


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


    private void MusicFadeDown()
    {
        print("Music fade down");

        if (m_musicMuted != null)
            m_musicMuted.TransitionTo(m_musicFadeTime);
    }


    private void MusicFadeUp()
    {
        print("Music fade up");

        if (m_musicFullVolume != null)
            m_musicFullVolume.TransitionTo(m_musicFadeTime);
    }


    void OnEnable()
    {
        EventManager.StartListening(StandardEventName.Pause, OnPause);
        EventManager.StartListening(StandardEventName.Unpause, OnUnpause);
        EventManager.StartListening(StandardEventName.ActivateCameraPan, MusicFadeUp);
        EventManager.StartListening(StandardEventName.MissionFailed, MusicFadeUp);
        EventManager.StartListening(StandardEventName.ReturnToMenu, MusicFadeUp);
        EventManager.StartListening(StandardEventName.StartMission, MusicFadeDown);
    }


    void OnDisable()
    {
        EventManager.StopListening(StandardEventName.Pause, OnPause);
        EventManager.StopListening(StandardEventName.Unpause, OnUnpause);
        EventManager.StopListening(StandardEventName.ActivateCameraPan, MusicFadeUp);
        EventManager.StopListening(StandardEventName.MissionFailed, MusicFadeUp);
        EventManager.StopListening(StandardEventName.ReturnToMenu, MusicFadeUp);
        EventManager.StopListening(StandardEventName.StartMission, MusicFadeDown);
    }
}

