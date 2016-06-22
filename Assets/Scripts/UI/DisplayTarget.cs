using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayTarget : MonoBehaviour {

    //Arrays
    private GameObject[] enemies;
    private GameObject[] radarDots;

    private GameObject radarTarget;
    public Text targetName;
    public Text targetDistance;

    private int targetIndex;
    private int enemiesLeft;

    public Transform playerTransform;

    private Vector3 direction;

    public Camera targetCamera;


    // Use this for initialization
    void Start () {

        //populate enemies array with all enemies tagged with Enemy
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        radarDots = new GameObject[enemies.Length];
        radarDots = GameObject.FindGameObjectsWithTag("Radar Dot");

        radarTarget = GameObject.Find("Target Canvas");

        //targetName = GameObject.Find("Target Name").GetComponent<Text>();
        //playerTransform = FindObjectOfType<PlayerFlyingInput>().transform;

        targetIndex = 0;


    }
	
	// Update is called once per frame
	void Update () {

        if (enemies[targetIndex] != null)
        {
            targetName.text = enemies[targetIndex].name;
            direction = enemies[targetIndex].transform.position - playerTransform.position;
            targetDistance.text = "Dist: " + (direction.magnitude / 100).ToString("f1");
            cameraTrackTarget();

        }
        else
        {
            //enemiesLeft--;
        }

        if (Input.GetKeyDown(KeyCode.T) && enemies[targetIndex] != null)
        {
            if (targetIndex < enemies.Length - 1)
            {   
                //radarTarget.transform.SetParent(radarDots[targetIndex].transform);
            }

            targetIndex++;

            if (targetIndex >= enemies.Length)
            {
                targetIndex = 0;
            }
        }
	
	}

    void cameraTrackTarget()
    {
        targetCamera.transform.position = (playerTransform.position + direction) * 2;
        targetCamera.transform.LookAt(enemies[targetIndex].transform.position);
        //targetCamera.transform.position = (playerTransform.position + direction);
    }
}
