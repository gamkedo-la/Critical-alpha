using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    private Canvas canvasComponent;


    void Start()
    {
		var canvasObject = GameObject.Find("Pause canvas");

		if (canvasObject == null)
			return;
		
		canvasComponent = canvasObject.GetComponent<Canvas>();
        canvasComponent.enabled = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

            if (Time.timeScale != 0)
            {
                Time.timeScale = 0;
                canvasComponent.enabled = true;
            }
            else
            {
                Time.timeScale = 1;
                canvasComponent.enabled = false;
            }

            Application.Quit();
	}


    public void LoadLevel(string LevelToLoad)
    {
        Time.timeScale = 1;
        Application.LoadLevel(LevelToLoad);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
