using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class DisplayTarget : MonoBehaviour {

    //Arrays
    private List<GameObject> enemies;
    private List<GameObject> radarDots;

    private GameObject radarTarget;

    private Transform playerTransform;

    private Text targetName;
    private Text targetDistance;
    private Text targetHealth;

    private EnemyHealth enemyHealth;

    private int targetIndex;
    private Vector3 direction;

    private Camera targetCamera;

    private Color targetBorderColor;

    private Image targetBorderTop;
    private Image targetBorderBottom;

    private GameObject targetSelectIcon;

    private bool initialised;


    // Use this for initialization
    void Start () {
        Initialise();   
    }


    public void Initialise() {
        if (initialised)
            return;

        targetIndex = 0;

        radarTarget = GameObject.Find("Target Canvas");

        var radar = FindObjectOfType<DisplayRadar>();
        radar.Initialise();

        enemies = radar.returnEnemyList();
        radarDots = radar.returnRadarDotList();

        targetName = GameObject.Find("Target Name").GetComponent<Text>();
        targetDistance = GameObject.Find("Target Distance").GetComponent<Text>();
        targetHealth = GameObject.Find("Target Health").GetComponent<Text>();

        playerTransform = FindObjectOfType<PlayerFlyingInput>().GetComponent<Transform>();
        targetCamera = GameObject.Find("Target Camera").GetComponent<Camera>();

        targetBorderTop = GameObject.Find("Target Border Top").GetComponent<Image>();
        targetBorderBottom = GameObject.Find("Target Border Bottom").GetComponent<Image>();

        updateBorderColor();

        targetSelectIcon = GameObject.Find("Target Select Icon");
        //targetSelectIcon.transform.SetAsLastSibling();

        initialised = true;
    }


    // Update is called once per frame
    void Update () {

        if (playerTransform == null)
            return;

        if (enemies[targetIndex] != null)
        {
            targetName.text = enemies[targetIndex].name;
            direction = enemies[targetIndex].transform.position - playerTransform.position;
            //targetSelectIcon.transform.SetParent(radarDots[targetIndex].transform);
            targetSelectIcon.transform.position = radarDots[targetIndex].transform.position;

            //Makes sure whatever colored dot you are over is rendered on top
            radarDots[targetIndex].transform.SetAsLastSibling();

            targetDistance.text = "Dist: " + (direction.magnitude / 100).ToString("f1");

            enemyHealth = enemies[targetIndex].GetComponent<EnemyHealth>();
            targetHealth.text = "Hull: " + ((float) enemyHealth.CurrentHealth / enemyHealth.StartingHealth) * 100 + "%";

            cameraTrackTarget();

        }
        else
        {
            targetIndex++;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            targetIndex++;

            if (targetIndex >= enemies.Count)
            {
                targetIndex = 0;
            }

            if (enemies[targetIndex].tag == "Enemy Air")
            {
                Debug.Log("Enemy Air");
            }

            updateBorderColor();

        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            targetIndex--;

            if (targetIndex < 0)
            {
                targetIndex = enemies.Count - 1;
            }

            updateBorderColor();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            float lowestMagnitude = direction.magnitude;

            for (int key = 0; key < enemies.Count; key++)
            {
                direction = enemies[key].transform.position - playerTransform.position;
                if(direction.magnitude < lowestMagnitude)
                {
                    lowestMagnitude = direction.magnitude;
                    targetIndex = key;
                }
            }

            updateBorderColor();
        }

        

        //Debug.Log("Target Index: " + targetIndex);

    }

    void cameraTrackTarget()
    {
        targetCamera.transform.position = enemies[targetIndex].transform.position - (direction.normalized * 15.0f);
        targetCamera.transform.LookAt(enemies[targetIndex].transform.position, Camera.main.transform.up);

    }

    void updateBorderColor()
    {
        targetBorderColor = radarDots[targetIndex].GetComponent<Image>().color;
        targetBorderTop.color = targetBorderColor;
        targetBorderBottom.color = targetBorderColor;
    }

    public GameObject returnCurrentTarget()
    {
        return enemies[targetIndex];
    }


}
