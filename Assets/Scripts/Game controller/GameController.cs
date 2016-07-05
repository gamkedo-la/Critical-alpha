using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    public void LoadLevel(string levelToLoad)
    {
        Time.timeScale = 1;
        LastMenuUsed.LastMenuUsedName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(levelToLoad);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}

