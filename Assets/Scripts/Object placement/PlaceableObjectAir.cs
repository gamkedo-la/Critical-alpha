using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FlyingControl))]
public class PlaceableObjectAir : PlaceableObject
{
    [Header("Air object options")]
    public float m_minAltitude = 20f;
    public float m_maxAltitude = 500f;
}
