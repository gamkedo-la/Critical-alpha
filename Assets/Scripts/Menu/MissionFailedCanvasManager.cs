using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class MissionFailedCanvasManager : MonoBehaviour
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
        //if (!MissionGoals.MissionSuccessful)
            StartCoroutine(EnableCanvasDelayed());
    }


    private IEnumerator EnableCanvasDelayed()
    {
        yield return new WaitForSeconds(m_delay);
        m_canvas.enabled = true;
    } 


    void OnEnable()
    {
        EventManager.StartListening(StandardEventName.MissionFailed, EnableCanvas);
    }


    void OnDisable()
    {
        EventManager.StopListening(StandardEventName.MissionFailed, EnableCanvas);
    }
}
