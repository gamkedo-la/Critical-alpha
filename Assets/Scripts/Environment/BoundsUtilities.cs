using UnityEngine;
using System.Collections;

public static class BoundsUtilities
{
	public static Bounds? OverallBounds(GameObject gameObject)
	{
		var colliders = gameObject.GetComponentsInChildren<Collider>();

		int length = colliders.Length;

		if (length == 0)
			return null;

		var newBounds = colliders[0].bounds;

		for (int i = 1; i < length; i++)
			newBounds.Encapsulate(colliders[i].bounds);

		return newBounds;
	}
}
