using UnityEngine;
using System.Collections;

public class ProceduralPlacementExample : MonoBehaviour 
{
	[SerializeField] MapGenerator m_mapGenerator;
	[SerializeField] GameObject[] m_testObjects;


	void Start()
	{
		if (m_mapGenerator != null) 
		{
			foreach (var testObject in m_testObjects)
			{
				var testObjectLocation = testObject.transform.position;
				float terrainHeight = m_mapGenerator.GetTerrainHeight(testObjectLocation.x, testObjectLocation.z);
		
				testObjectLocation.y = terrainHeight;

				testObject.transform.position = testObjectLocation;
			}
		}
	}
}
