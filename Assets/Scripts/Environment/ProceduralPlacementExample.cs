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
				float z = (j - m_numObjectsPerSide / 2) * m_separation + transform.position.z;

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
				var collider = testObject.GetComponentInChildren<Collider>();	// Note: this only works if the object has a single collider

				if (collider != null)
				{
					var bounds = collider.bounds;

					var originToMin = bounds.min - testObjectLocation;
					var originToMax = bounds.max - testObjectLocation;

					float originAboveBase = testObjectLocation.y - bounds.min.y;

					float rotationAngle = Random.Range(0f, 360f);
					testObject.transform.Rotate(Vector3.up * rotationAngle);

					originToMin = Quaternion.Euler(0, rotationAngle, 0) * originToMin;
					originToMax = Quaternion.Euler(0, rotationAngle, 0) * originToMax;

					var min = testObjectLocation + originToMin;
					var max = testObjectLocation + originToMax;

					float terrainHeightCorner1 = m_mapGenerator.GetTerrainHeight(min.x, min.z);
					float terrainHeightCorner2 = m_mapGenerator.GetTerrainHeight(min.x, max.z);
					float terrainHeightCorner3 = m_mapGenerator.GetTerrainHeight(max.x, min.z);
					float terrainHeightCorner4 = m_mapGenerator.GetTerrainHeight(max.x, max.z);

//					print(string.Format("{0}, {1}, {2}", min.x, min.z, terrainHeightCorner1));
//					print(string.Format("{0}, {1}, {2}", min.x, max.z, terrainHeightCorner2));
//					print(string.Format("{0}, {1}, {2}", max.x, min.z, terrainHeightCorner3));
//					print(string.Format("{0}, {1}, {2}", max.x, max.z, terrainHeightCorner4));

					float y = Mathf.Min(terrainHeightCorner1, terrainHeightCorner2, terrainHeightCorner3, terrainHeightCorner4);

					if (y < 0f)
						Destroy(testObject);
					else
					{
						testObjectLocation.y = y + originAboveBase;
						testObject.transform.position = testObjectLocation;
					}
				}
//				else
//				{
//					float terrainHeight = m_mapGenerator.GetTerrainHeight(testObjectLocation.x, testObjectLocation.z);
//
//					testObjectLocation.y = terrainHeight;
//				}
//
				//float terrainHeightAtOrigin = m_mapGenerator.GetTerrainHeight(0f, 0f);
				//print(terrainHeightAtOrigin);
			}
		}
	}
}
