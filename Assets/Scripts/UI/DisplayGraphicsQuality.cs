using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayGraphicsQuality : MonoBehaviour
{
    private Text m_text;

    void Awake()
    {
        m_text = GetComponent<Text>();
    }


    private void UpdateText(string qualityName)
    {
        m_text.text = string.Format("Quality: {0}", qualityName);
    }


    void OnEnable()
    {
        EventManager.StartListening(StringEventName.ChangeGraphicsQuality, UpdateText);
    }


    void OnDisable()
    {
        EventManager.StopListening(StringEventName.ChangeGraphicsQuality, UpdateText);
    }
}
