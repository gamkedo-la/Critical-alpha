using UnityEngine;
using System.Collections;

public class EarthPosition : MonoBehaviour {

    public float distance = 1000;
    public float scale = 15f;

    private Transform playerTransform;

	// Use this for initialization
	void Start () {

        playerTransform = FindObjectOfType<PlayerFlyingInput>().transform;

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, distance);
        transform.localScale = new Vector3(scale, scale, scale);
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, playerTransform.position.z + distance);

    }
}
