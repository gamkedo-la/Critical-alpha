using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class MissionManager : MonoBehaviour
{
    public static ProceduralPlacement MissionToLoad;

    [SerializeField] Text m_missionNameText;
    [SerializeField] Text m_missionStoryText;
    [SerializeField] ProceduralPlacement[] m_missions;



    void Awake()
    {
        if (m_missions.Length == 0)
            return;

        if (m_missionNameText != null)
            m_missionNameText.text = GetMissionName(0);

        if (m_missionStoryText != null)
            m_missionStoryText.text = GetMissionStory(0);

        MissionToLoad = m_missions[0];
    }

	
    public void SetMission(int index)
    {
        MissionToLoad = m_missions[index];

        if (m_missionNameText != null)
            m_missionNameText.text = GetMissionName(index);

        if (m_missionStoryText != null)
            m_missionStoryText.text = GetMissionStory(index);
    }


    public string GetMissionName(int index)
    {
        return m_missions[index].missionName;
    }


    public string GetMissionStory(int index)
    {
        return m_missions[index].missionStory;
    }
}
