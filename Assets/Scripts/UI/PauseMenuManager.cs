using UnityEngine;
using System.Collections;

public class PauseMenuManager : MonoBehaviour
{
    public void SetGraphicsQuality()
    {
        EventManager.TriggerEvent(StandardEventName.SetGraphicsQuality);
    }


    public void SetTerrainDetail()
    {
        EventManager.TriggerEvent(StandardEventName.SetTerrainDetail);
    }
}
