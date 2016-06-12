using UnityEngine;
using System.Collections;

public class PlaceableObjectGround : PlaceableObject
{
    [Header("Ground object options")]
    public float m_minHeight = 0f;
    public float m_maxHeight = 1000f;
    public float m_maxGradientDeg = 20f; 
}
