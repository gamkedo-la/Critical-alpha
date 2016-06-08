using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QualityButton : MonoBehaviour {

    private Text m_text;
    private QualityController qualityController;

    void Awake()
    {
        m_text = GetComponentInChildren<Text>();
        qualityController = FindObjectOfType<QualityController>();
    }

    void Update()
    {
        m_text.text = string.Format("Quality: {0}", qualityController.GetGraphicsQuality());
    }

}
