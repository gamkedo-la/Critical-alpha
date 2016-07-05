using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LastMenuUsed : MonoBehaviour
{
    public static string LastMenuUsedName;


    public void ReturnToLastMenuUsed()
    {
        SceneManager.LoadScene(LastMenuUsedName);
    }


    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }


    public void RestartMission()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
