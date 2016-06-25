using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayRadar : MonoBehaviour
{


    public float radarRadius;
    private Transform frontRadarPanelTransform;
    private Transform rearRadarPanelTransform;
    private Transform playerTransform;

    private float yawDiff = 0;
    private float pitchDiff = 0;

    private float blipFromCenterScale;
    private float polarPtOnRadarX;
    private float polarPtOnRadarY;

    private GameObject[] enemies;
    private GameObject[] radarDots;

    public GameObject radarBlip;

    private Vector3 targetRelative;
    private float angleAngToBlip;

    public Color enemyAirColor;
    public Color enemySeaColor;
    public Color enemyLandColor;
    public Color noTargetColor;

    // Use this for initialization
    void Start()
    {

        //blip = GameObject.Find("Radar Blip").GetComponent<RectTransform>();
        frontRadarPanelTransform = GameObject.Find("Front Radar Panel").GetComponent<RectTransform>();
        rearRadarPanelTransform = GameObject.Find("Rear Radar Panel").GetComponent<RectTransform>();

        playerTransform = FindObjectOfType<PlayerFlyingInput>().transform;

        //populate enemies array with all enemies tagged with Enemy
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        radarDots = new GameObject[enemies.Length];

        //Instantiate radar blips for each enemy;
        for (int key = 0; key < enemies.Length; key++)
        {
            radarDots[key] = (GameObject)Instantiate(radarBlip, frontRadarPanelTransform.transform.position, frontRadarPanelTransform.transform.rotation);
            radarDots[key].transform.SetParent(frontRadarPanelTransform);
            radarDots[key].transform.localScale = new Vector3(0.5F, 0.5f, 1);

            //Debug.Log(enemies[key].name);
        }


    }

    // Update is called once per frame
    void Update()
    {
        for (int key = 0; key < enemies.Length; key++)
        {
            //If enemy hasn't been destroyed, update radar dot position, otherwise destroy
            if (enemies[key] != null)
            {


                targetRelative = playerTransform.InverseTransformDirection(enemies[key].transform.position - playerTransform.position);

                CalculatePolarPoints();

                if (yawDiff >= -90 && yawDiff <= 90 && pitchDiff >= -90 && pitchDiff <= 90)
                {

                    radarDots[key].GetComponent<RectTransform>().SetParent(frontRadarPanelTransform);
                    radarDots[key].GetComponent<RectTransform>().anchoredPosition = new Vector2(polarPtOnRadarX, polarPtOnRadarY);
                }
                else
                {

                    targetRelative = playerTransform.InverseTransformDirection(playerTransform.position - enemies[key].transform.position);

                    CalculatePolarPoints();

                    //Use negatives to mirror the view for the Rear Radar
                    radarDots[key].GetComponent<RectTransform>().SetParent(rearRadarPanelTransform);
                    radarDots[key].GetComponent<RectTransform>().anchoredPosition = new Vector2(-polarPtOnRadarX, -polarPtOnRadarY);
                }
            }
            else
            {
                GameObject.Destroy(radarDots[key]);
            }
           
        }
    }

    private void CalculatePolarPoints()
    {

        yawDiff = Mathf.Atan2(targetRelative.x, targetRelative.z) * Mathf.Rad2Deg;
        pitchDiff = Mathf.Atan2(targetRelative.y, targetRelative.z) * Mathf.Rad2Deg;

        //Debug.Log("yawDiff = " + yawDiff + "    pitchDiff = " + pitchDiff);

        blipFromCenterScale = Mathf.Max(Mathf.Abs(pitchDiff), Mathf.Abs(yawDiff)) / 90.0f;

        angleAngToBlip = Mathf.Atan2(pitchDiff, yawDiff);

        polarPtOnRadarX = blipFromCenterScale * radarRadius * Mathf.Cos(angleAngToBlip);
        polarPtOnRadarY = blipFromCenterScale * radarRadius * Mathf.Sin(angleAngToBlip);

    }

    public GameObject[] returnEnemiesArray()
    {
        return enemies;
    }

    public GameObject[] returnRadarDotsArray()
    {
        return radarDots;
    }

}
