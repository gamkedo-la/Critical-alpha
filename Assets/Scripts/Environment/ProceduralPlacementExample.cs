using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralPlacementExample : MonoBehaviour 
{
	[SerializeField] MapGenerator m_mapGenerator;
	[SerializeField] GameObject m_testPrefab;
	[SerializeField] float m_separation = 50f;
	[SerializeField] int m_numObjectsPerSide = 9;

	private List<GameObject> m_testObjects;


	void Awake()
	{
		m_testObjects = new List<GameObject>();

		if (m_testPrefab == null)
			return;

		for (int i = 0; i < m_numObjectsPerSide; i++)
		{
			float x = (i - m_numObjectsPerSide / 2) * m_separation + transform.position.x;

			for (int j = 0; j < m_numObjectsPerSide; j++)
			{
				float z = (j - m_numObjectsPerSide / 2) * m_separation + transform.position.y;

				var newObject = (GameObject) Instantiate(m_testPrefab, new Vector3(x, 0, z), Quaternion.identity);
				newObject.transform.parent = transform;
				m_testObjects.Add(newObject);
			}
		}
	}


	void Start()
	{
		if (m_mapGenerator != null) 
		{
			foreach (var testObject in m_testObjects)
			{
				var testObjectLocation = testObject.transform.position;
				var collider = testObject.GetComponent<Collider>();

				if (collider != null)
				{
					var bounds = collider.bounds;
					var min = bounds.min;
					var max = bounds.max;
					float centreAboveBase = transform.position.y - min.y;

					float terrainHeightCorner1 = m_mapGenerator.GetTerrainHeight(min.x, min.z);
					float terrainHeightCorner2 = m_mapGenerator.GetTerrainHeight(min.x, max.z);
					float terrainHeightCorner3 = m_mapGenerator.GetTerrainHeight(max.x, min.z);
					float terrainHeightCorner4 = m_mapGenerator.GetTerrainHeight(max.x, max.z);

					testObjectLocation.y = Mathf.Min(terrainHeightCorner1, 
						terrainHeightCorner2, terrainHeightCorner3, terrainHeightCorner4) + centreAboveBase;
				}
				else
				{
					float terrainHeight = m_mapGenerator.GetTerrainHeight(testObjectLocation.x, testObjectLocation.z);

					testObjectLocation.y = terrainHeight;
				}

				testObject.transform.position = testObjectLocation;
			}
		}
	}
}
