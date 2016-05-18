using UnityEngine;
using System.Collections;

public class TerrainEquationSlope : TerrainEquationBase
{
    [SerializeField] float m_xScale = 1f;
    [SerializeField] float m_yScale = 1f;


    public override float GetHeight(float x, float y)
    {
        float height = x * m_xScale + y * m_yScale;

        return height;
    }
}
