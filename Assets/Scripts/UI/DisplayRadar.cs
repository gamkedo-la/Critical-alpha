using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayRadar : MonoBehaviour {

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

    // Use this for initialization
    void Start () {

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

            Vector3 eulerAnglesTo = Quaternion.FromToRotation(transform.forward,
                                                angleToward.transform.position - transform.position).eulerAngles;

            yawDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.y);
            pitchDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.z);

            /*if(transform.forward.x >= 0){
                pitchDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.z);
            }else{
                pitchDiff = Mathf.DeltaAngle(eulerAnglesTo.z, 0f);
            }*/

            Debug.Log(
                //yawDiff + ", " + pitchDiff + ", " + Mathf.DeltaAngle(0f, eulerAnglesTo.z) + ", " + 
                transform.forward
            );

            //blip.anchoredPosition = new Vector2(yawDiff, pitchDiff);
            frontRadarPanelTransform.rotation = Quaternion.Euler(0, 0, -playerTransform.eulerAngles.z);



            if (yawDiff >= -90 && yawDiff <= 90 && pitchDiff >= -90 && pitchDiff <= 90)
            {

                CalculatePolarPoints();

                blip.SetParent(frontRadarPanelTransform);
                blip.anchoredPosition = new Vector2(polarPtOnRadarX, polarPtOnRadarY);
            }
            else
            {

                Debug.Log("Rear Camera");

                //reset the yaw and diff for the rear camera using -transform.forward to get a value between -90 and 90
                eulerAnglesTo = Quaternion.FromToRotation(-transform.forward,
                                    angleToward.transform.position - transform.position).eulerAngles;

                yawDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.y);
                pitchDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.z);

                /*if (transform.forward.x >= 0)
                {
                    pitchDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.z);
                }
                else
                {
                    pitchDiff = Mathf.DeltaAngle(eulerAnglesTo.z, 0f);
                }*/

                rearRadarPanelTransform.rotation = Quaternion.Euler(0, 0, -playerTransform.eulerAngles.z);

                CalculatePolarPoints();

                blip.SetParent(rearRadarPanelTransform);
                blip.anchoredPosition = new Vector2(-polarPtOnRadarX, polarPtOnRadarY);
            }


            //Debug.Log(
            //    polarPtOnRadarX + ", " + polarPtOnRadarY 
            //);

        }

    }


   private void CalculatePolarPoints()
   {
        angOfPtOnRadar = Mathf.Atan2(pitchDiff, yawDiff);
        blipFromCenterScale = Mathf.Max(Mathf.Abs(pitchDiff), Mathf.Abs(yawDiff)) / 90.0f;
        polarPtOnRadarX = blipFromCenterScale * radarRadius * Mathf.Cos(angOfPtOnRadar);
        polarPtOnRadarY = blipFromCenterScale * radarRadius * Mathf.Sin(angOfPtOnRadar);

    }
}
