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

        initialised = true;
    }


    // Update is called once per frame
    void LateUpdate () {

        if (playerTransform == null)
            return;

        if (enemies.Count == 0)
        {
            targetName.text = "No Target";
            targetDistance.text = " ";
            targetHealth.text = " ";
            targetBorderTop.color = Color.white;
            targetBorderBottom.color = Color.white;
            targetSelectIcon.active = false;
            return;
        }

        if (targetIndex > enemies.Count - 1)
        {
            targetIndex = 0;
            Debug.Log("Reset To Zero");
        }

        if (enemies[targetIndex] != null)
        {

            //enemies[targetIndex].layer = LayerMask.NameToLayer("Target");
            //targetCamera.cullingMask = 1 << 10;
            

            //Gets object bounds of mesh and then chooses whichever is largest, x y or z.
            var overallBounds = enemies[targetIndex].GetComponent<PlaceableObject>().GetUnrotatedBounds();
            var largestBound = 0f;
            if(overallBounds.Value.size.x > overallBounds.Value.size.y && overallBounds.Value.size.x > overallBounds.Value.size.z)
            {

                largestBound = overallBounds.Value.size.x;

            }
            else if(overallBounds.Value.size.y > overallBounds.Value.size.x && overallBounds.Value.size.y > overallBounds.Value.size.z){
                largestBound = overallBounds.Value.size.y;
            }
            else
            {
                largestBound = overallBounds.Value.size.z;
            }

            Debug.Log("Overall Bounds Max: " + largestBound);
            targetCamera.orthographicSize = (largestBound / 2f) + 5.5f;

            targetSelectIcon.active = true;
            updateBorderColor();

            targetName.text = enemies[targetIndex].name;
            direction = enemies[targetIndex].transform.position - playerTransform.position;
            //targetSelectIcon.transform.SetParent(radarDots[targetIndex].transform);
            targetSelectIcon.transform.position = radarDots[targetIndex].transform.position;

            //Makes sure whatever colored dot you are over is rendered on top
            radarDots[targetIndex].transform.SetAsLastSibling();

            targetDistance.text = "Dist: " + (direction.magnitude / 100).ToString("f1");

            enemyHealth = enemies[targetIndex].GetComponent<EnemyHealth>();
            targetHealth.text = "Hull: " + Mathf.Ceil(((float) enemyHealth.CurrentHealth / enemyHealth.StartingHealth) * 100) + "%";

            cameraTrackTarget();

        }
        else
        {
            GameObject.Destroy(radarDots[targetIndex]);
            enemies.Remove(enemies[targetIndex]);
            radarDots.Remove(radarDots[targetIndex]);
            Debug.Log("Enemies Length: " + enemies.Count);
            //targetIndex++;
        }


        if (Input.GetKeyDown(KeyCode.T))
        {
            //enemies[targetIndex].layer = LayerMask.NameToLayer("Enemy");

            targetIndex++;

            if (targetIndex >= enemies.Count)
            {
                targetIndex = 0;
            }

            if (enemies[targetIndex].tag == "Enemy Air")
            {
                Debug.Log("Enemy Air");
            }

            //updateBorderColor();

        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            //enemies[targetIndex].layer = LayerMask.NameToLayer("Enemy");

            targetIndex--;

            if (targetIndex < 0)
            {
                targetIndex = enemies.Count - 1;
            }

            //updateBorderColor();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            //enemies[targetIndex].layer = LayerMask.NameToLayer("Enemy");

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

            //updateBorderColor();
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
        if(targetIndex < enemies.Count)
            return enemies[targetIndex];
        else
            return null;
    }


}
