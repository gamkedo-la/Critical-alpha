using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class MissionManager : MonoBehaviour
{
    public static ProceduralPlacement MissionToLoad;

    [SerializeField] Text m_missionNameText;
    [SerializeField] Text m_missionStoryText;
    [SerializeField] Text m_missionGoalText;
    [SerializeField] ProceduralPlacement[] m_missions;


    void Awake()
    {
        if (m_missions.Length == 0)
            return;

        SetMission(0);
    }

	
    public void SetMission(int index)
    {
        MissionToLoad = m_missions[index];

        if (m_missionNameText != null)
            m_missionNameText.text = GetMissionName(index);

        if (m_missionStoryText != null)
            m_missionStoryText.text = GetMissionStory(index);

        if (m_missionGoalText != null)
            m_missionGoalText.text = GetMissionGoal(index);
    }


    public string GetMissionName(int index)
    {
        return m_missions[index].missionName;
    }


    public string GetMissionStory(int index)
    {
        return m_missions[index].missionStory;
    }


    public string GetMissionGoal(int index)
    {
        return m_missions[index].missionGoal;
    }
}
