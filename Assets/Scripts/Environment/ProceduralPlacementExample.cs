using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralPlacementExample : MonoBehaviour 
{
	[SerializeField] GameObject m_testPrefab;
	[SerializeField] float m_separation = 50f;
	[SerializeField] int m_numObjectsPerSide = 9;

	private List<GameObject> m_testObjects;
    private MapGenerator m_mapGenerator;


    void Awake()
	{
        var mapGeneratorObject = GameObject.FindGameObjectWithTag(Tags.MapGenerator);

        if (mapGeneratorObject != null)
            m_mapGenerator = mapGeneratorObject.GetComponent<MapGenerator>();

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
				var rigidbody = testObject.GetComponent<Rigidbody>();

				var testObjectLocation = testObject.transform.position;
				//var collider = testObject.GetComponentInChildren<Collider>();	// Note: this only works if the object has a single collider

				var bounds = BoundsUtilities.OverallBounds(testObject);

				if (bounds != null)
				{
                    float minY = bounds.Value.min.y;
                    float minX = bounds.Value.min.x;
                    float minZ = bounds.Value.min.z;
                    float maxX = bounds.Value.max.x;
                    float maxZ = bounds.Value.max.z;

                    var corner1 = new Vector3(minX, minY, minZ);
                    var corner2 = new Vector3(minX, minY, maxZ);
                    var corner3 = new Vector3(maxX, minY, minZ);
                    var corner4 = new Vector3(maxX, minY, maxZ);

                    var originToCorner1 = corner1 - testObjectLocation;
                    var originToCorner2 = corner2 - testObjectLocation;
                    var originToCorner3 = corner3 - testObjectLocation;
                    var originToCorner4 = corner4 - testObjectLocation;

                    float originAboveBase = testObjectLocation.y - bounds.Value.min.y;

                    float rotationY = Random.Range(0f, 360f);
                    var rotation = Quaternion.Euler(0, rotationY, 0);
                    testObject.transform.rotation = rotation;

                    originToCorner1 = rotation * originToCorner1;
                    originToCorner2 = rotation * originToCorner2;
                    originToCorner3 = rotation * originToCorner3;
                    originToCorner4 = rotation * originToCorner4;

                    corner1 = testObjectLocation + originToCorner1;
                    corner2 = testObjectLocation + originToCorner2;
                    corner3 = testObjectLocation + originToCorner3;
                    corner4 = testObjectLocation + originToCorner4;

                    float terrainHeightCorner1 = m_mapGenerator.GetTerrainHeight(corner1.x, corner1.z);
                    float terrainHeightCorner2 = m_mapGenerator.GetTerrainHeight(corner2.x, corner2.z);
                    float terrainHeightCorner3 = m_mapGenerator.GetTerrainHeight(corner3.x, corner3.z);
                    float terrainHeightCorner4 = m_mapGenerator.GetTerrainHeight(corner4.x, corner4.z);

//					print(string.Format("{0}, {1}, {2}", min.x, min.z, terrainHeightCorner1));
//					print(string.Format("{0}, {1}, {2}", min.x, max.z, terrainHeightCorner2));
//					print(string.Format("{0}, {1}, {2}", max.x, min.z, terrainHeightCorner3));
//					print(string.Format("{0}, {1}, {2}", max.x, max.z, terrainHeightCorner4));

					float y = rigidbody == null  
						? Mathf.Min(terrainHeightCorner1, terrainHeightCorner2, terrainHeightCorner3, terrainHeightCorner4)
						: Mathf.Max(terrainHeightCorner1, terrainHeightCorner2, terrainHeightCorner3, terrainHeightCorner4);

					if (y < 0f)
						Destroy(testObject);
					else
					{
						y += originAboveBase;

						testObjectLocation.y = y;
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
