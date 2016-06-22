using UnityEngine;
using System.Collections;

public class SpinPropeller : MonoBehaviour
{
    [SerializeField] float m_minSpinSpeed = 1000f;
    [SerializeField] float m_maxSpinSpeed = 3000f;  

    private FlyingControl m_flyingControlScript;
    private float m_maxForwardSpeed;
    private float m_spinSpeed;


    void Awake()
    {
        m_flyingControlScript = transform.root.gameObject.GetComponent<FlyingControl>();
        m_maxForwardSpeed = m_flyingControlScript.MaxForwardSpeed;
    }


    void Update()
    {
        if (m_flyingControlScript == null)
        { 
            transform.Rotate(Vector3.forward, m_maxSpinSpeed * Time.deltaTime);
            return;
        }

        float forwardSpeed = m_flyingControlScript.ForwardSpeed;

        m_spinSpeed = m_minSpinSpeed + forwardSpeed * (m_maxSpinSpeed - m_minSpinSpeed) / m_maxForwardSpeed;

        transform.Rotate(Vector3.forward, m_spinSpeed * Time.deltaTime);
    }
}
