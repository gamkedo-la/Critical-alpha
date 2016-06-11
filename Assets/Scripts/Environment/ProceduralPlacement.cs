using UnityEngine;
using System.Collections;

public class ProceduralPlacement : MonoBehaviour
{
    [Header("Seeding")]
    [SerializeField] int m_seed = 1;

    [Header("Main target")]
    [SerializeField] PlaceableObject m_mainTarget;
    [SerializeField] float m_minDistanceFromPlayer = 800f;
    [SerializeField] float m_maxDistanceFromPlayer = 1200f;

    [Header("Enemy aircraft")]
    [SerializeField] PlaceableObject[] m_aircraftTypes;
    [SerializeField] int m_numberOfAircraft = 10;

    [Header("Enemy ground defences")]
    [SerializeField] PlaceableObject[] m_groundDefenceTypes;
    [SerializeField] int m_numberOfGroundDefences = 20;

    private MapGenerator m_mapGenerator;
    private Vector3 m_player;


    void Awake()
    {
        var mapGeneratorObject = GameObject.FindGameObjectWithTag(Tags.MapGenerator);

        if (mapGeneratorObject != null)
            m_mapGenerator = mapGeneratorObject.GetComponent<MapGenerator>();
    }


    void Start()
    {
        if (m_mapGenerator == null)
            return;

        Random.seed = m_seed;

        var playerObject = GameObject.FindGameObjectWithTag(Tags.Player);

        if (playerObject != null)
            m_player = playerObject.transform.position;

        PlaceMainTarget();
        PlaceEnemyAircraft();
    }


    private void PlaceMainTarget()
    {
        if (m_mainTarget == null)
            return;

        var newObject = (GameObject) Instantiate(m_mainTarget.gameObject, Vector3.zero, Quaternion.identity);
        newObject.transform.parent = transform;

        bool success = false;
        int attempts = 0;

        while (!success)
        {
            success = TestPosition(newObject);
            attempts++;
        }

        print(string.Format("Main target took {0} attempts to place", attempts));
    }


    private void PlaceEnemyAircraft()
    {
        if (m_aircraftTypes.Length == 0)
            return;
    }


    private void PlaceEnemyGroundDefences()
    {
        if (m_groundDefenceTypes == null)
            return;
    }


    private bool TestPosition(GameObject testObject)
    {
        float distance = Random.Range(m_minDistanceFromPlayer, m_maxDistanceFromPlayer);
        float rotationY = Random.Range(0f, 360f);
        
        var position = m_player + Vector3.forward * distance;
        var rotation = Quaternion.Euler(0, rotationY, 0);

        testObject.transform.position = position;
        testObject.transform.rotation = rotation;

        //var rigidbody = testObject.GetComponent<Rigidbody>();

        var testObjectLocation = testObject.transform.position;

        var bounds = BoundsUtilities.OverallBounds(testObject);

        bool success = true;

        if (bounds != null)
        {
            var originToMin = bounds.Value.min - testObjectLocation;
            var originToMax = bounds.Value.max - testObjectLocation;

            float originAboveBase = testObjectLocation.y - bounds.Value.min.y;

            originToMin = rotation * originToMin;
            originToMax = rotation * originToMax;

            var min = testObjectLocation + originToMin;
            var max = testObjectLocation + originToMax;

            float terrainHeightCorner1 = m_mapGenerator.GetTerrainHeight(min.x, min.z);
            float terrainHeightCorner2 = m_mapGenerator.GetTerrainHeight(min.x, max.z);
            float terrainHeightCorner3 = m_mapGenerator.GetTerrainHeight(max.x, min.z);
            float terrainHeightCorner4 = m_mapGenerator.GetTerrainHeight(max.x, max.z);

            float minHeight = Mathf.Min(terrainHeightCorner1, terrainHeightCorner2, terrainHeightCorner3, terrainHeightCorner4);
            float maxHeight = Mathf.Max(terrainHeightCorner1, terrainHeightCorner2, terrainHeightCorner3, terrainHeightCorner4);

            success = maxHeight < 0;

            //float y = rigidbody == null
            //    ? Mathf.Min(terrainHeightCorner1, terrainHeightCorner2, terrainHeightCorner3, terrainHeightCorner4)
            //    : Mathf.Max(terrainHeightCorner1, terrainHeightCorner2, terrainHeightCorner3, terrainHeightCorner4);

            //if (y < 0f)
            //    Destroy(testObject);
            //else
            //{
            //    y += originAboveBase;

                testObjectLocation.y = 0;
                testObject.transform.position = testObjectLocation;
            //}
        }

        return success;
    }
}
