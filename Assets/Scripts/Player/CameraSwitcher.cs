using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] Transform[] m_cameraPositions;

    private Transform m_cameraTransform;
    private int m_index;


    void Awake()
    {
        m_cameraTransform = Camera.main.transform;
    }

	
	void Update()
    {
        int index = m_index;

        if (Input.GetKeyDown(KeyCode.Alpha1) && m_cameraPositions.Length > 0)
            index = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2) && m_cameraPositions.Length > 1)
            index = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3) && m_cameraPositions.Length > 2)
            index = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4) && m_cameraPositions.Length > 3)
            index = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha5) && m_cameraPositions.Length > 4)
            index = 4;

        if (m_index != index)
        {
            m_index = index;
            var newTransform = m_cameraPositions[m_index];

            m_cameraTransform.position = newTransform.position;
            m_cameraTransform.rotation = newTransform.rotation;

            if (m_index == 0)
                EventManager.TriggerEvent(BooleanEventName.ActivateHud, true);
            else
                EventManager.TriggerEvent(BooleanEventName.ActivateHud, false);
        }
    }
}
