using UnityEngine;
using System.Collections;

public class SetUpCullingDistances : MonoBehaviour
{
	void Start() 
	{
		var camera = Camera.main;
		float[] distances = new float[32];

		distances[9] = 1000;		// Small object
		distances[10] = 1250;		// Medium object
		distances[11] = 1500;		// Large object

		camera.layerCullDistances = distances;
		camera.layerCullSpherical = true;
	}
}
