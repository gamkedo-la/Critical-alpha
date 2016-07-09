using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1;
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
}

