using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayRadar : MonoBehaviour {

    public GameObject angleToward;
    public float radarRadius;
    public RectTransform blip;
    private Transform radarPanelTransform;
    private Transform playerTransform;

    // Use this for initialization
    void Start () {

        blip = GameObject.Find("Radar Blip").GetComponent<RectTransform>();
        radarPanelTransform = GameObject.Find("Radar Panel").GetComponent<RectTransform>();
        playerTransform = FindObjectOfType<PlayerFlyingInput>().transform;

    }
	
	// Update is called once per frame
	void Update () {

        if (angleToward != null)
        {
            Vector3 eulerAnglesTo = Quaternion.FromToRotation(transform.forward,
                                                angleToward.transform.position - transform.position).eulerAngles;

            float yawDiff = Mathf.DeltaAngle(0f, eulerAnglesTo.y);
            float pitchDiff = Mathf.DeltaAngle(eulerAnglesTo.x, 0f);

            //Debug.Log(
            //    yawDiff + ", " + pitchDiff
            //);

            //blip.anchoredPosition = new Vector2(yawDiff, pitchDiff);
            radarPanelTransform.rotation = Quaternion.Euler(0, 0, -playerTransform.eulerAngles.z);

            var angOfPtOnRadar = Mathf.Atan2(pitchDiff, yawDiff);
            var polarPtOnRadarX = radarRadius * Mathf.Cos(angOfPtOnRadar);
            var polarPtOnRadarY = radarRadius * Mathf.Sin(angOfPtOnRadar);

            var magnitude = (yawDiff * yawDiff) + (pitchDiff * pitchDiff);
            //var magnitude = (polarPtOnRadarX * polarPtOnRadarX) + (polarPtOnRadarY * polarPtOnRadarY);

            //polarPtOnRadarX *= magnitude;
            //polarPtOnRadarY *= magnitude;

            blip.anchoredPosition = new Vector2(polarPtOnRadarX, polarPtOnRadarY);

            Debug.Log(
                polarPtOnRadarX + ", " + polarPtOnRadarY 
            );

        }
    
    }
}
