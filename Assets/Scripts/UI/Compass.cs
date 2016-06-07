using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Compass : MonoBehaviour {

    private RawImage compassRawImage;
    private Transform playerTransform;
    private float offsetX;



	// Use this for initialization
	void Start () {
        compassRawImage = GetComponent<RawImage>();
        playerTransform = FindObjectOfType<PlayerFlyingInput>().transform;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Direction: " + playerTransform.transform.eulerAngles.y);

        offsetX = playerTransform.transform.eulerAngles.y;

        //Divide by 360 because uvRect range is from 0 to 1
        offsetX /= 360;

        //Keep all the values the same except for X
        compassRawImage.uvRect = new Rect(offsetX, compassRawImage.uvRect.y, compassRawImage.uvRect.width, compassRawImage.uvRect.height);



    }
}
