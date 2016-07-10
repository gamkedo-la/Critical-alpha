using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class PauseMenuManager : MonoBehaviour
{
    private Canvas m_canvasComponent;
    private bool m_inputInUse;


    void Awake()
    {
        m_canvasComponent = GetComponent<Canvas>();
        m_canvasComponent.enabled = false;
    }


    void Update()
    {
        if (Input.GetAxisRaw("Pause") == 1
            && !PlayerHealth.PlayerDead
            && !MissionGoals.MissionSuccessful)
        {
            if (!m_inputInUse)
            {
                m_inputInUse = true;

                if (Time.timeScale != 0)
                {
                    Time.timeScale = 0;
                    m_canvasComponent.enabled = true;
                }
                else
                {
                    Time.timeScale = 1;
                    m_canvasComponent.enabled = false;
                }
            }
        }
        
        if (Input.GetAxisRaw("Pause") != 1)
            m_inputInUse = false;
    }


    public void LoadLevel(string levelToLoad)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(levelToLoad);
    }


    public void QuitGame()
    {
        Application.Quit();
    }


    public void SetGraphicsQuality()
    {
        EventManager.TriggerEvent(StandardEventName.SetGraphicsQuality);
    }


    public void SetTerrainDetail()
    {
        EventManager.TriggerEvent(StandardEventName.SetTerrainDetail);
    }
}
