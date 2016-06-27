using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TrailRenderer))]
public class TrailController : MonoBehaviour
{
    private TrailRenderer m_trail;
    private float m_time;


    void Awake()
    {
        m_trail = GetComponent<TrailRenderer>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
