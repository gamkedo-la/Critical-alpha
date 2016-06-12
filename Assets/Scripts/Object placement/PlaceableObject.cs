using UnityEngine;
using System.Collections;

public class PlaceableObject : MonoBehaviour
{
    [Header("General options")]
    public float m_minSeparation = 50f;
    public float m_minDistFromMainTarget = 100f;
    public float m_maxDistFromMainTarget = 1000f;
}
