using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour
{
    [SerializeField] float m_speed = 2000;

	void Update()
    {
        transform.Rotate(Vector3.forward, m_speed * Time.deltaTime);
	}
}
