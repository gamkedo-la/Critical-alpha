using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] cloudPrefabs;

    [Header("Distribution")]
    [SerializeField] int m_numberOfClouds = 30;
    [Space(10)]
    [SerializeField] float m_minAltitude = 50f;
    [SerializeField] float m_maxAltitude = 200f;
    [Space(10)]
    [SerializeField] float m_maxDistance = 1500f;
    [SerializeField] float m_minSeparation = 200f;

    [Header("Appearance")] 
    [SerializeField] float m_minScale = 3f;
    [SerializeField] float m_maxScale = 10f;
    [Space(10)]
    [SerializeField] bool m_fadeWithDistance = true;
    [SerializeField] float m_distanceOfMinAlpha = 1400f;
    [SerializeField] float m_disatnceOfMaxAlpha = 800f;
    [Space(10)]
    [Range(0f, 1f)]
    [SerializeField] float m_minAlpha = 0f;
    [Range(0f, 1f)]
    [SerializeField] float m_maxAlpha = 1f;
    [Space(10)]
    [SerializeField] Vector3 m_rotationAxis = Vector3.forward;	//Default for the low-poly pack

    private GameObject m_cloudParent;
    private float m_maxDistanceSq;
    private float m_distanceOfMaxAlphaSq;
    private float m_distanceOfMinAlphaSq;
    private Vector2 m_direction;
    private Color m_cloudColour;
    private Camera m_mainCamera;
    private Vector2 m_origin;
    private Dictionary<GameObject, MeshRenderer[]> m_clouds;

    public static Dictionary<GameObject, Vector2> AllClouds;


	void Start () 
	{
        m_cloudParent = gameObject;

		if (AllClouds == null)
			AllClouds = new Dictionary<GameObject, Vector2>();

		m_mainCamera = Camera.main;
		var cameraOrigin = m_mainCamera.transform.position;
		m_origin.Set(cameraOrigin.x, cameraOrigin.z);

		m_clouds = new Dictionary<GameObject, MeshRenderer[]>();

        m_maxDistanceSq = m_maxDistance * m_maxDistance;
		m_distanceOfMaxAlphaSq = m_disatnceOfMaxAlpha * m_disatnceOfMaxAlpha;
		m_distanceOfMinAlphaSq = m_distanceOfMinAlpha * m_distanceOfMinAlpha;

		for (int i = 0; i < m_numberOfClouds; i++) 
		{
			int index = Random.Range(0, cloudPrefabs.Length);
			var newCloud = Instantiate(cloudPrefabs[index]);

			var position = GetSpawnPosition();
			float y = Random.Range(m_minAltitude, m_maxAltitude);
			newCloud.transform.position = new Vector3(position.x, y, position.y);

			float scale = Random.Range(m_minScale, m_maxScale);
			newCloud.transform.localScale = new Vector3(scale, scale, scale);

			var rotation = Random.Range(0f, 356f) * m_rotationAxis;
			newCloud.transform.Rotate(rotation);

			var renderers = newCloud.GetComponentsInChildren<MeshRenderer>();
			m_clouds.Add(newCloud, renderers);

            AllClouds.Add(newCloud, position);

            if (m_cloudParent != null)
                newCloud.transform.parent = m_cloudParent.transform;
		}
	}


	private Vector2 GetSpawnPosition() 
	{
		Vector2 spawnPosition = new Vector2();
		float startTime = Time.realtimeSinceStartup;
		bool test = false;
		
		while (test == false) 
		{
			test = true;
			spawnPosition = Random.insideUnitCircle * m_maxDistance + m_origin;
			
			foreach (var cloud in AllClouds.Values)
			{
				float distance = (cloud - spawnPosition).magnitude;
				
				if (distance < m_minSeparation)
					test = false;
			}
			
			if (Time.realtimeSinceStartup - startTime > 0.1f) 
			{
				print("Time out placing Cloud!");
				return Vector3.zero;
			}
		}
		
		return spawnPosition;
	}


	void Update () 
	{
		foreach (var cloud in m_clouds)
		{
			cloud.Key.transform.Translate(Time.deltaTime * WeatherController.windSpeed, Space.World);

			var direction3 = m_mainCamera.transform.position - cloud.Key.transform.position;

			m_direction.Set(direction3.x, direction3.z);

			float distanceSq = m_direction.sqrMagnitude;

            if (m_fadeWithDistance)
            {
                var renderers = cloud.Value;
                UpdateCloudAlpha(renderers, distanceSq);
            }

            if (distanceSq > m_maxDistanceSq)
			{
				m_direction.Normalize();
				float newX = cloud.Key.transform.position.x + (m_direction.x * m_maxDistance * 2f);
				float newY = cloud.Key.transform.position.y;
				float newZ = cloud.Key.transform.position.z + (m_direction.y * m_maxDistance * 2f);

				cloud.Key.transform.position = new Vector3(newX, newY, newZ);
			}
		}
	}


    private void UpdateCloudAlpha(MeshRenderer[] renderers, float distanceSq)
    {
        float alphaFraction = distanceSq < m_distanceOfMaxAlphaSq
                ? 1f
                : 1f - ((distanceSq - m_distanceOfMaxAlphaSq) / (m_distanceOfMinAlphaSq - m_distanceOfMaxAlphaSq));

        float alpha = alphaFraction * (m_maxAlpha - m_minAlpha) + m_minAlpha;
        alpha = Mathf.Clamp01(alpha);

        foreach (var renderer in renderers)
        {
            if (alpha > 0.999)
                renderer.material.SetInt("_Mode", 0);
            else
                renderer.material.SetInt("_Mode", 2);

            m_cloudColour = renderer.material.color;
            m_cloudColour.a = alpha;
            renderer.material.color = m_cloudColour;
        }
    }
}
