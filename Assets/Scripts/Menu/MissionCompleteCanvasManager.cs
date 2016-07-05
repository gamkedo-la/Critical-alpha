using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class MissionCompleteCanvasManager : MonoBehaviour
{
    [SerializeField] float m_delay = 3f;

    private Canvas m_canvas;

    
    void Awake()
    {
        m_canvas = GetComponent<Canvas>();
        m_canvas.enabled = false;
    }


    private void EnableCanvas()
    {
        StartCoroutine(EnableCanvasDelayed());
    }


    private IEnumerator EnableCanvasDelayed()
    {
        yield return new WaitForSeconds(m_delay);
        m_canvas.enabled = true;
    }


    void OnEnable()
    {
        EventManager.StartListening(StandardEventName.MissionSuccessful, EnableCanvas);
    }


    void OnDisable()
    {
        EventManager.StopListening(StandardEventName.MissionSuccessful, EnableCanvas);
    }
}
