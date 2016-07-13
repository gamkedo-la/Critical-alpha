using UnityEngine;
using System.Collections;

public class ObjectBounds : MonoBehaviour
{
    public Bounds? bounds; 


	void Awake()
    {
        bounds = BoundsUtilities.OverallBounds(gameObject);
    }
}
