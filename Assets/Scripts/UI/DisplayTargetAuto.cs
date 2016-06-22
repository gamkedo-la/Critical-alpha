using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class DisplayTargetAuto : MonoBehaviour {

    //Arrays
    private GameObject[] enemies;
    private GameObject[] radarDots;

    private GameObject radarTarget;

    private Transform playerTransform;

    private Text targetName;
    private Text targetDistance;

    private int enemiesLeft;
    private int targetIndex;
    private Vector3 direction;

    private Camera targetCamera;

    // Use this for initialization
    void Start () {

        targetIndex = 5;

        radarTarget = GameObject.Find("Target Canvas");

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        targetName = GameObject.Find("Target Name").GetComponent<Text>();
        targetDistance = GameObject.Find("Target Distance").GetComponent<Text>();
        playerTransform = FindObjectOfType<PlayerFlyingInput>().GetComponent<Transform>();
        targetCamera = GameObject.Find("Target Camera").GetComponent<Camera>();
        enemiesLeft = enemies.Length;
    }

    // Update is called once per frame
    void Update () {
        Debug.Log("Enemies Left = " + enemiesLeft);
        Debug.Log("Target Index = " + targetIndex);

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
        
    }
}
