using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayRadarBackup : MonoBehaviour
{

    public GameObject angleToward;
    public float radarRadius;
    public RectTransform blip;
    private Transform frontRadarPanelTransform;
    private Transform rearRadarPanelTransform;
    private Transform playerTransform;

    private float yawDiff = 0;
    private float pitchDiff = 0;

    private float angOfPtOnRadar;
    private float blipFromCenterScale;
    private float polarPtOnRadarX;
    private float polarPtOnRadarY;

    private GameObject[] enemies;
    public GameObject radarBlip;
    public Transform target;
    private Vector3 targetRelative;

    // Use this for initialization
    void Start()
    {

        blip = GameObject.Find("Radar Blip").GetComponent<RectTransform>();
        frontRadarPanelTransform = GameObject.Find("Front Radar Panel").GetComponent<RectTransform>();
        rearRadarPanelTransform = GameObject.Find("Rear Radar Panel").GetComponent<RectTransform>();

        playerTransform = FindObjectOfType<PlayerFlyingInput>().transform;

        /*enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            GameObject radarDot = (GameObject)Instantiate(radarBlip, frontRadarPanelTransform.transform.position, frontRadarPanelTransform.transform.rotation);
            radarDot.transform.SetParent(frontRadarPanelTransform);

            Debug.Log(enemy.name);

        }*/



    }

    // Update is called once per frame
    void Update()
    {

        if (angleToward != null)
        {
            target = angleToward.transform;
            targetRelative = transform.InverseTransformDirection(angleToward.transform.position - transform.position);




            yawDiff = Mathf.Atan2(targetRelative.x, targetRelative.z) * Mathf.Rad2Deg;
            pitchDiff = Mathf.Atan2(targetRelative.y, targetRelative.z) * Mathf.Rad2Deg;


            Debug.Log("yawDiff = " + yawDiff + "    pitchDiff = " + pitchDiff);

            blipFromCenterScale = Mathf.Max(Mathf.Abs(pitchDiff), Mathf.Abs(yawDiff)) / 90.0f;

            float angleAngToBlip = Mathf.Atan2(pitchDiff, yawDiff);

            polarPtOnRadarX = blipFromCenterScale * radarRadius * Mathf.Cos(angleAngToBlip);
            polarPtOnRadarY = blipFromCenterScale * radarRadius * Mathf.Sin(angleAngToBlip);





            if (yawDiff >= -90 && yawDiff <= 90 && pitchDiff >= -90 && pitchDiff <= 90)
            {




                //CalculatePolarPoints();

                blip.SetParent(frontRadarPanelTransform);
                blip.anchoredPosition = new Vector2(polarPtOnRadarX, polarPtOnRadarY);
            }
            else
            {

                Debug.Log("Rear Camera");

                targetRelative = transform.InverseTransformDirection(transform.position - angleToward.transform.position);




                yawDiff = Mathf.Atan2(targetRelative.x, targetRelative.z) * Mathf.Rad2Deg;
                pitchDiff = Mathf.Atan2(targetRelative.y, targetRelative.z) * Mathf.Rad2Deg;


                Debug.Log("yawDiff = " + yawDiff + "    pitchDiff = " + pitchDiff);

                blipFromCenterScale = Mathf.Max(Mathf.Abs(pitchDiff), Mathf.Abs(yawDiff)) / 90.0f;

                angleAngToBlip = Mathf.Atan2(pitchDiff, yawDiff);

                polarPtOnRadarX = blipFromCenterScale * radarRadius * Mathf.Cos(angleAngToBlip);
                polarPtOnRadarY = blipFromCenterScale * radarRadius * Mathf.Sin(angleAngToBlip);




                blip.SetParent(rearRadarPanelTransform);
                blip.anchoredPosition = new Vector2(-polarPtOnRadarX, -polarPtOnRadarY);
            }


        }

    }

}
