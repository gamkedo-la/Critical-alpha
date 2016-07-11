using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Button))]
public class ButtonSelectManager : MonoBehaviour
{
    private Button m_thisButton;
    private EventSystem m_eventSystem;


	void Awake()
    {
        m_thisButton = GetComponent<Button>();
        m_eventSystem = FindObjectOfType<EventSystem>();
    }


    public void OnButtonSelected()
    {
        m_eventSystem.SetSelectedGameObject(null);
        m_thisButton.Select();
    }
}
