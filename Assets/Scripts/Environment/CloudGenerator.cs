using UnityEngine;

using System.Collections.Generic;

public class CloudGenerator : MonoBehaviour
{
    [SerializeField] CloudProperties[] m_cloudProperties;

    [Header("Overall distribution")]
    [SerializeField] float m_maxDistance = 1500f;

    [Header("Overall appearance")]
    [SerializeField] bool m_fadeWithDistance = true;
    [SerializeField] float m_distanceOfMinAlpha = 1400f;
    [SerializeField] float m_disatnceOfMaxAlpha = 800f;
    [Space(10)]
    [Range(0f, 1f)]
    [SerializeField] float m_minAlpha = 0f;
    [Range(0f, 1f)]
    [SerializeField] float m_maxAlpha = 1f;

    private GameObject m_cloudParent;
    private float m_maxDistanceSq;
    private float m_distanceOfMaxAlphaSq;
    private float m_distanceOfMinAlphaSq;
    private Vector2 m_direction;
    private Color m_cloudColour;
    private Camera m_mainCamera;
    private Vector2 m_origin;
    private Dictionary<GameObject, MeshRenderer[]> m_cloudRenderers;
    private Dictionary<GameObject, Vector2> m_cloudPositions;


	void Start () 
	{
        m_cloudParent = gameObject;
        m_cloudRenderers = new Dictionary<GameObject, MeshRenderer[]>();
        m_cloudPositions = new Dictionary<GameObject, Vector2>();

		m_mainCamera = Camera.main;
		var cameraOrigin = m_mainCamera.transform.position;
		m_origin.Set(cameraOrigin.x, cameraOrigin.z);

        m_maxDistanceSq = m_maxDistance * m_maxDistance;

        for (int j = 0; j < m_cloudProperties.Length; j++)
        {
            var cloud = m_cloudProperties[j];

            int numberOfClouds = cloud.numberOfClouds;
            m_distanceOfMaxAlphaSq = m_disatnceOfMaxAlpha * m_disatnceOfMaxAlpha;
            m_distanceOfMinAlphaSq = m_distanceOfMinAlpha * m_distanceOfMinAlpha;

            for (int i = 0; i < numberOfClouds; i++)
            {
                var position = GetSpawnPosition(cloud.minSeparation);

                if (!position.HasValue)
                    continue;

                var newCloud = Instantiate(cloud.cloudPrefab);

                float y = Random.Range(cloud.minAltitude, cloud.maxAltitude);
                newCloud.transform.position = new Vector3(position.Value.x, y, position.Value.y);

                float scale = Random.Range(cloud.minScale, cloud.maxScale);
                newCloud.transform.localScale = new Vector3(scale, scale, scale);

                var rotation = Random.Range(0f, 356f) * cloud.rotationAxis;
                newCloud.transform.Rotate(rotation);

                var renderers = newCloud.GetComponentsInChildren<MeshRenderer>();
                m_cloudRenderers.Add(newCloud, renderers);
                m_cloudPositions.Add(newCloud, position.Value);

                if (m_cloudParent != null)
                    newCloud.transform.parent = m_cloudParent.transform;
            }
        }
	}


	private Vector2? GetSpawnPosition(float minSeparation) 
	{
		Vector2 spawnPosition = new Vector2();
		float startTime = Time.realtimeSinceStartup;
		bool test = false;
		
		while (test == false) 
		{
			test = true;
			spawnPosition = Random.insideUnitCircle * m_maxDistance + m_origin;
			
			foreach (var cloud in m_cloudPositions.Values)
			{
				float distance = (cloud - spawnPosition).magnitude;
				
				if (distance < minSeparation)
					test = false;
			}
			
			if (Time.realtimeSinceStartup - startTime > 0.1f) 
			{
				print("Time out placing Cloud!");
				return null;
			}
		}
		
		return spawnPosition;
	}


	void Update () 
	{
		foreach (var cloud in m_cloudRenderers)
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
