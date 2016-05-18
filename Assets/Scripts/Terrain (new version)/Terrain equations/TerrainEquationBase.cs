using UnityEngine;
using System.Collections;

public abstract class TerrainEquationBase : MonoBehaviour, ITerrainEquation
{
    public virtual void Initialise()
    {

    }


    public virtual float GetHeight(float x, float y)
    {
        return 0f;
    }
}
