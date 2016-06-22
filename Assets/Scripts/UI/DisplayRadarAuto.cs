using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayRadarAuto : MonoBehaviour {

    public float radarRadius;
    private Transform frontRadarPanelTransform;
    private Transform rearRadarPanelTransform;
    private Transform playerTransform;

    private float yawDiff = 0;
    private float pitchDiff = 0;

    private float angOfPtOnRadar;
    private float blipFromCenterScale;
    private float polarPtOnRadarX;
    private float polarPtOnRadarY;

    //Prefab
    public GameObject radarBlip;

    //Arrays
    private GameObject[] enemies;
    private GameObject[] radarDots;

 
    // Use this for initialization
    void Start () {

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
            radarDots[key].transform.localScale = new Vector3(0.5f, 0.5f, 1);

            //Debug.Log(enemies[key].name);
        }



    }

    // Update is called once per frame
    void Update()
    {

        for(int key = 0; key < enemies.Length; key++)
        {      
            //If enemy hasn't been destroyed, update radar dot position, otherwise destroy
            if (enemies[key] != null)
            {
                Vector3 eulerAnglesTo = Quaternion.FromToRotation(transform.forward,
                                                    enemies[key].transform.position - transform.position).eulerAngles;

                yawDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.y);
                pitchDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.z);

                /*if(transform.forward.x >= 0){
                    pitchDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.z);
                }else{
                    pitchDiff = Mathf.DeltaAngle(eulerAnglesTo.z, 0f);
                }*/

                //Debug.Log(
                //eulerAnglesTo.y + ", " + eulerAnglesTo.z +
                //yawDiff + 
                //", " + pitchDiff 
                //transform.forward
                //);

                //blip.anchoredPosition = new Vector2(yawDiff, pitchDiff);
                frontRadarPanelTransform.rotation = Quaternion.Euler(0, 0, -playerTransform.eulerAngles.z);



                if (yawDiff >= -90 && yawDiff <= 90 && pitchDiff >= -90 && pitchDiff <= 90)
                {

                    CalculatePolarPoints();

                    radarDots[key].GetComponent<RectTransform>().SetParent(frontRadarPanelTransform);
                    radarDots[key].GetComponent<RectTransform>().anchoredPosition = new Vector2(polarPtOnRadarX, polarPtOnRadarY);
                }
                else
                {

                    Debug.Log("Rear Camera");

                    //reset the yaw and diff for the rear camera using -transform.forward to get a value between -90 and 90
                    eulerAnglesTo = Quaternion.FromToRotation(-transform.forward,
                                        enemies[key].transform.position - transform.position).eulerAngles;

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

                    radarDots[key].GetComponent<RectTransform>().SetParent(rearRadarPanelTransform);
                    radarDots[key].GetComponent<RectTransform>().anchoredPosition = new Vector2(-polarPtOnRadarX, polarPtOnRadarY);
                }


                //Debug.Log(
                //    polarPtOnRadarX + ", " + polarPtOnRadarY 
                //);
            }
            else
            {
                GameObject.Destroy(radarDots[key]);
            }

        }

    }


   private void CalculatePolarPoints()
   {
        angOfPtOnRadar = Mathf.Atan2(pitchDiff, yawDiff);
        blipFromCenterScale = Mathf.Max(Mathf.Abs(pitchDiff), Mathf.Abs(yawDiff)) / 90.0f;
        polarPtOnRadarX = blipFromCenterScale * radarRadius * Mathf.Cos(angOfPtOnRadar);
        polarPtOnRadarY = blipFromCenterScale * radarRadius * Mathf.Sin(angOfPtOnRadar);

        //Debug.Log(
        //    angOfPtOnRadar 
        //);

    }
}
