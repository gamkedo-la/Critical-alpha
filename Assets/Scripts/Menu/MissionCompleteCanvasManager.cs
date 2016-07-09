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
        if (!PlayerHealth.PlayerDead)
            StartCoroutine(EnableCanvasDelayed());
    }


    private IEnumerator EnableCanvasDelayed()
    {
        yield return new WaitForSeconds(m_delay);

        if (!PlayerHealth.PlayerDead)
        {
            m_canvas.enabled = true;
            EventManager.TriggerEvent(StandardEventName.ActivateCameraPan);
        }
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
