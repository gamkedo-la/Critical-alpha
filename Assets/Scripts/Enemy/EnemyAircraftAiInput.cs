using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FlyingControl))]
public class EnemyAircraftAiInput : MonoBehaviour
{
    private FlyingControl m_flyingControlScript;


    void Awake()
    {
        m_flyingControlScript = GetComponent<FlyingControl>();
    }
}
