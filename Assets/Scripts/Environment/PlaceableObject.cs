using UnityEngine;
using System.Collections;

public class PlaceableObject : MonoBehaviour
{
    public enum PlaceableObjectType
    {
        Ground = 0,
        Water = 1,
        Air = 2,
    }
	

    [SerializeField] PlaceableObjectType m_type;
    [SerializeField] float m_minSeparation = 50f;

    [Header("Ground type options")]
    [SerializeField] float m_minHeight = 0f;
    [SerializeField] float m_maxHeight = 1000f;
    [SerializeField] float m_maxGroundGradientDeg = 20f; 

    //[Header("Water type options")]

    //[Header("Air type options")]
}
