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
            //targetRelative = transform.InverseTransformPoint(target.transform.position);
            targetRelative = transform.InverseTransformDirection(angleToward.transform.position - transform.position);

            //Debug.Log("Target Relative: " + targetRelative);

            //Vector3 eulerAnglesTo = Quaternion.FromToRotation(transform.forward,
            //                                    targetRelative - transform.position).eulerAngles;


            yawDiff = Mathf.Atan2(targetRelative.x, targetRelative.z);
            pitchDiff = Mathf.Atan2(targetRelative.y, targetRelative.z);
            blipFromCenterScale = Mathf.Max(Mathf.Abs(pitchDiff), Mathf.Abs(yawDiff)) / 90;
            polarPtOnRadarX = blipFromCenterScale * radarRadius * Mathf.Cos(yawDiff);
            polarPtOnRadarY = blipFromCenterScale * radarRadius * Mathf.Sin(pitchDiff);

            //yawDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.x);
            //pitchDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.y);



            /*if(transform.forward.x >= 0){
                pitchDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.z);
            }else{
                pitchDiff = Mathf.DeltaAngle(eulerAnglesTo.z, 0f);
            }*/

            Debug.Log(
            
            //eulerAnglesTo.y + ", " + eulerAnglesTo.z + ", " + eulerAnglesTo.x
            //yawDiff +
            //", " + pitchDiff
            polarPtOnRadarX + ", " + polarPtOnRadarY
            //transform.forward
            );

            //blip.anchoredPosition = new Vector2(yawDiff, pitchDiff);
            //frontRadarPanelTransform.rotation = Quaternion.Euler(0, 0, -playerTransform.eulerAngles.z);



            if (yawDiff >= -90 && yawDiff <= 90 && pitchDiff >= -90 && pitchDiff <= 90)
            {




                //CalculatePolarPoints();

                //blip.SetParent(frontRadarPanelTransform);
                //Sblip.anchoredPosition = new Vector2(polarPtOnRadarX, polarPtOnRadarY);
            }
            else
            {

                Debug.Log("Rear Camera");

                //reset the yaw and diff for the rear camera using -transform.forward to get a value between -90 and 90
                //eulerAnglesTo = Quaternion.FromToRotation(-transform.forward,
                //                    angleToward.transform.position - transform.position).eulerAngles;

                //yawDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.y);
                //pitchDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.z);

                /*if (transform.forward.x >= 0)
                {
                    pitchDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.z);
                }
                else
                {
                    pitchDiff = Mathf.DeltaAngle(eulerAnglesTo.z, 0f);
                }*/

                //rearRadarPanelTransform.rotation = Quaternion.Euler(0, 0, -playerTransform.eulerAngles.z);

                //CalculatePolarPoints();

                //blip.SetParent(rearRadarPanelTransform);
                //blip.anchoredPosition = new Vector2(-polarPtOnRadarX, polarPtOnRadarY);
            }


            //Debug.Log(
            //    polarPtOnRadarX + ", " + polarPtOnRadarY 
            //);

        }

    }


    private void CalculatePolarPoints()
    {

        //angOfPtOnRadar = Mathf.Atan2(pitchDiff, yawDiff);
        blipFromCenterScale = Mathf.Max(Mathf.Abs(pitchDiff), Mathf.Abs(yawDiff)) / 90.0f;
        //polarPtOnRadarX = blipFromCenterScale * radarRadius * Mathf.Cos(angOfPtOnRadar);
        //polarPtOnRadarY = blipFromCenterScale * radarRadius * Mathf.Sin(angOfPtOnRadar);

        polarPtOnRadarX = blipFromCenterScale * radarRadius * Mathf.Cos(angOfPtOnRadar);
        polarPtOnRadarY = blipFromCenterScale * radarRadius * Mathf.Sin(angOfPtOnRadar);

        //Debug.Log(
        //    angOfPtOnRadar
        //);

    }
}
