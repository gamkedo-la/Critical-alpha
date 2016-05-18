using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class FlyingControl : MonoBehaviour
{
    public Vector3 ForwardVelocity;

    [SerializeField] float m_minForwardSpeed = 10f;
    [SerializeField] float m_maxForwardSpeed = 100f;
    [SerializeField] float m_forwardSpeed = 30f;
    [SerializeField] float m_liftMultiplier = 0.1f;

    [Range(0f, 100f)]
    [SerializeField] float m_accelerationRate = 50f;

    [SerializeField] float m_bankRate = 90f;
    [SerializeField] float m_pitchRate = 90f;
    [SerializeField] float m_turnRate = 10f;

    [SerializeField] Slider m_slider;

    private float m_liftSpeed;


	void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        float a = Input.GetAxis("Acceleration");
        a = m_accelerationRate * a * Time.deltaTime;

        m_forwardSpeed += a;
        m_forwardSpeed = Mathf.Clamp(m_forwardSpeed, m_minForwardSpeed, m_maxForwardSpeed);
        m_liftSpeed = m_forwardSpeed * m_liftMultiplier;

        ForwardVelocity = Vector3.forward * m_forwardSpeed;
        transform.Translate((ForwardVelocity + Vector3.up * m_liftSpeed) * Time.deltaTime);

        transform.Rotate(transform.right, v * Time.deltaTime * m_pitchRate, Space.World);
        transform.Rotate(-transform.forward, h * Time.deltaTime * m_bankRate, Space.World);

        var forwardOnGround = Vector3.ProjectOnPlane(transform.forward, Vector3.up);//.normalized;
        var rightOnGround = new Vector3(forwardOnGround.z, 0, -forwardOnGround.x);
        float bankDot = Vector3.Dot(rightOnGround, transform.up);
        float turnAmount = bankDot * m_turnRate;

        //float bankAngle = transform.rotation.eulerAngles.z;
        //float turnAmount = -Mathf.Sin(bankAngle * Mathf.Deg2Rad) * m_turnRate;

        transform.Rotate(Vector3.up, turnAmount * Time.deltaTime, Space.World);

        if (m_slider != null)
        {
            m_slider.minValue = m_minForwardSpeed;
            m_slider.maxValue = m_maxForwardSpeed;
            m_slider.value = m_forwardSpeed;
        }
    }
}
