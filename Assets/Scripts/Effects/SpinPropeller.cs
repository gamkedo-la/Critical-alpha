using UnityEngine;
using System.Collections;

public class SpinPropeller : MonoBehaviour
{
    [SerializeField] Vector2 m_spinSpeedMinMax = new Vector2(1000f, 3000f); 

    private FlyingControl m_flyingControlScript;
    private float m_maxForwardSpeed;
    private float m_spinSpeed;


    void Awake()
    {
        m_flyingControlScript = gameObject.GetComponentInParent<FlyingControl>();
        m_maxForwardSpeed = m_flyingControlScript.MaxForwardSpeed;
    }


    void Update()
    {
        if (m_flyingControlScript == null)
        { 
            transform.Rotate(Vector3.forward, m_spinSpeedMinMax.y * Time.deltaTime);
            return;
        }

        float forwardSpeed = m_flyingControlScript.ForwardSpeed;

        m_spinSpeed = Mathf.Lerp(m_spinSpeedMinMax.x, m_spinSpeedMinMax.y, forwardSpeed / m_maxForwardSpeed);

        transform.Rotate(Vector3.forward, m_spinSpeed * Time.deltaTime);
    }
}
