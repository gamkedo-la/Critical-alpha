﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] Transform[] m_cameraPositions;
    [SerializeField] int[] m_indicesToShowHud = new int[] { 0, 1 };
    [SerializeField] float m_cameraSwitchCooldown = 0.3f;

    private Transform m_cameraTransform;
    private int m_index;
    private double m_timeSinceSwitched;


    void Awake()
    {
        m_cameraTransform = Camera.main.transform;
    }

	
	void Update()
    {
        m_timeSinceSwitched += Time.unscaledDeltaTime;
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
        else if (Input.GetKeyDown(KeyCode.Alpha6) && m_cameraPositions.Length > 5)
            index = 5;
        else if (Input.GetKeyDown(KeyCode.Alpha7) && m_cameraPositions.Length > 6)
            index = 6;
        else if (Input.GetKeyDown(KeyCode.Alpha8) && m_cameraPositions.Length > 7)
            index = 7;
        else if (Input.GetKeyDown(KeyCode.Alpha9) && m_cameraPositions.Length > 8)
            index = 8;
        else if (Input.GetKeyDown(KeyCode.Alpha0) && m_cameraPositions.Length > 9)
            index = 9;

        if (m_timeSinceSwitched > m_cameraSwitchCooldown)
        {
            int increment = (int) Input.GetAxisRaw("Camera");
            index += increment;
            index = (index + m_cameraPositions.Length) % m_cameraPositions.Length;
        }

        if (m_index != index)
        {
            m_timeSinceSwitched = 0;

            m_index = index;
            var newTransform = m_cameraPositions[m_index];

            m_cameraTransform.position = newTransform.position;
            m_cameraTransform.rotation = newTransform.rotation;
            m_cameraTransform.parent = newTransform;

            EventManager.TriggerEvent(BooleanEventName.ActivateHud, false);

            for (int i = 0; i < m_indicesToShowHud.Length; i++)
            {
                int indexToShow = m_indicesToShowHud[i];

                if (m_index == indexToShow)
                    EventManager.TriggerEvent(BooleanEventName.ActivateHud, true);             
            }
        }   
    }
}
