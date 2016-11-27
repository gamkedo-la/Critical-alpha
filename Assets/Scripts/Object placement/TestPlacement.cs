using UnityEngine;
using System.Collections;

public class TestPlacement : MonoBehaviour
{
    [SerializeField] MapGenerator m_mapGenerator;


	void Start()
    {
        if (m_mapGenerator == null)
            m_mapGenerator = GameObject.FindGameObjectWithTag(Tags.MapGenerator).GetComponent<MapGenerator>();

        float height = m_mapGenerator.GetTerrainHeight(transform.position.x, transform.position.z);

        var position = transform.position;
        position.y = height;

        transform.position = position;
	}
}
