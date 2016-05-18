using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayTerrainDetail : MonoBehaviour
{
    private Text text;


    void Awake()
    {
        text = GetComponent<Text>();
    }


    void Start()
    {
        TerrainDetailChanged((int) QualityController.TerrainDetail);
    }


    private void TerrainDetailChanged(int terrainDetailLevel)
    {
        text.text = string.Format("Terrain: {0}", (TerrainDetail) terrainDetailLevel);
    }


    void OnEnable()
    {
        EventManager.StartListening(IntegerEventName.ChangeTerrainDetail, TerrainDetailChanged);
    }


    void OnDisable()
    {
        EventManager.StopListening(IntegerEventName.ChangeTerrainDetail, TerrainDetailChanged);
    }
}
