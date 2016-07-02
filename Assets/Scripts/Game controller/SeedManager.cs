using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SeedManager : MonoBehaviour
{
    public static string TerrainSeedString;
    public static int TerrainSeed;
    public static string MissionSeedString;
    public static int MissionSeed;

    [SerializeField] InputField m_terrainSeedText;
    [SerializeField] InputField m_missionSeedText;
	

    void Start()
    {
        print("Seed mamager awake");

        if (TerrainSeedString == null)
            TerrainSeedString = TerrainSeed.ToString();

        if(MissionSeedString == null)
            MissionSeedString = MissionSeed.ToString();

        if (m_terrainSeedText != null)
        {
            m_terrainSeedText.text = TerrainSeedString;
            //print(string.Format("Terrain seed: {0} ({1}, {2})", TerrainSeed, TerrainSeedString, m_terrainSeedText.text));
        }

        if (m_missionSeedText != null)
        {
            m_missionSeedText.text = MissionSeedString;
            //print(string.Format("Mission seed: {0} ({1})", MissionSeed, MissionSeedString));
        }
    }


    public void SetTerrainSeed(string input)
    {
        int seed = 0;

        if (!int.TryParse(input, out seed))
            seed = input.GetHashCode();

        TerrainSeed = seed;
        TerrainSeedString = input;

        //print(string.Format("Terrain seed: {0} ({1})", TerrainSeed, TerrainSeedString));
    }


    public void SetMissionSeed(string input)
    {
        int seed = 0;

        if (!int.TryParse(input, out seed))
            seed = input.GetHashCode();

        MissionSeed = seed;
        MissionSeedString = input;

        //print(string.Format("Mission seed: {0} ({1})", MissionSeed, MissionSeedString));
    }
}
