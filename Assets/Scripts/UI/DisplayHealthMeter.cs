using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayHealthMeter : MonoBehaviour {

    PlayerHealth playerHealth;

    private Image healthMeter;
    private Image damageIndicator;
    private float currentFillAmount;
    private float previousFillAmount;

	// Use this for initialization
	void Start () {
        playerHealth = FindObjectOfType<PlayerHealth>();

        healthMeter = GameObject.Find("Health Meter").GetComponent<Image>();
        damageIndicator = GameObject.Find("Damage Indicator").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {

        currentFillAmount = (float)playerHealth.CurrentHealth / playerHealth.StartingHealth;
        healthMeter.fillAmount = currentFillAmount;

        if (currentFillAmount <= 0)
            damageIndicator.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.587f);
        else if (currentFillAmount < previousFillAmount)
            StartCoroutine("FlashDamage");

        previousFillAmount = currentFillAmount;

    }

    IEnumerator FlashDamage()
    {
        damageIndicator.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.587f);
        yield return new WaitForSeconds(0.1f);
        damageIndicator.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0.587f);


    }
}
