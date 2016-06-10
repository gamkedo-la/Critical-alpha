using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplaySpeed : MonoBehaviour {

    private Slider speedSlider;
    private Text speedText;

    //Convert speed to more useful unit, like mph or kph
    public float speedMultiplier;

	// Use this for initialization
	void Start () {

        speedSlider = GameObject.Find("Speed Slider").GetComponent<Slider>();
        speedText = GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {

        speedText.text = Mathf.Ceil(speedSlider.value * speedMultiplier).ToString();

	}
}
