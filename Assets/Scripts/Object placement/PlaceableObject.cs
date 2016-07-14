using UnityEngine;
using System.Collections;

public class PlaceableObject : MonoBehaviour
{
    public Bounds? bounds;


    void Awake()
    {
        var rotation = transform.rotation;
        transform.rotation = Quaternion.identity;

        bounds = BoundsUtilities.OverallBounds(gameObject);

        transform.rotation = rotation;
    }
}
