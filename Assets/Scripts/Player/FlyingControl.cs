﻿using UnityEngine;
using System.Collections;
using System;

public class FlyingControl : MonoBehaviour
{
    public Vector3 ForwardVelocity;

    [SerializeField] float m_minForwardSpeed = 10f;
    [SerializeField] float m_maxForwardSpeed = 100f;
    [SerializeField] float m_forwardSpeed = 60f;
    [SerializeField] float m_liftMultiplier = 0f;
	[SerializeField] float m_downSpeed = 0f;

    [Range(0f, 100f)]
    [SerializeField] float m_accelerationRate = 50f;

    [SerializeField] float m_bankRate = 120f;
    [SerializeField] float m_pitchRate = 90f;
    public float turnRate = 20f;

    private bool m_isPlayer;
    private float m_liftSpeed;
    private float m_previousForwardSpeed;

    private float m_v;
    private float m_h;
    private float m_a;


    public float BankRate { get { return m_bankRate; } }


    void Start()
    {
        m_isPlayer = CompareTag(Tags.Player);

        if (m_isPlayer)
        {
            EventManager.TriggerEvent(FloatEventName.SetMinThrustLevel, m_minForwardSpeed);
            EventManager.TriggerEvent(FloatEventName.SetMaxThrustLevel, m_maxForwardSpeed);
            EventManager.TriggerEvent(FloatEventName.SetThrustLevel, m_forwardSpeed);
        }
    }


	void Update()
    {    
        m_a = m_accelerationRate * m_a * Time.deltaTime;

        m_forwardSpeed += m_a;
        m_forwardSpeed = Mathf.Clamp(m_forwardSpeed, m_minForwardSpeed, m_maxForwardSpeed);
        m_liftSpeed = m_forwardSpeed * m_liftMultiplier;

        ForwardVelocity = Vector3.forward * m_forwardSpeed;
        transform.Translate((ForwardVelocity + Vector3.up * m_liftSpeed) * Time.deltaTime);
		transform.Translate(m_downSpeed * Vector3.down * Time.deltaTime, Space.World);

        transform.Rotate(transform.right, m_v * Time.deltaTime * m_pitchRate, Space.World);
        transform.Rotate(-transform.forward, m_h * Time.deltaTime * m_bankRate, Space.World);

        var forwardOnGround = Vector3.ProjectOnPlane(transform.forward, Vector3.up);//.normalized;
        var rightOnGround = new Vector3(forwardOnGround.z, 0, -forwardOnGround.x);
        float bankDot = Vector3.Dot(rightOnGround, transform.up);
        float turnAmount = bankDot * turnRate;

        //float bankAngle = transform.rotation.eulerAngles.z;
        //float turnAmount = -Mathf.Sin(bankAngle * Mathf.Deg2Rad) * m_turnRate;

        transform.Rotate(Vector3.up, turnAmount * Time.deltaTime, Space.World);

        if (m_isPlayer && m_previousForwardSpeed != m_forwardSpeed)
            EventManager.TriggerEvent(FloatEventName.SetThrustLevel, m_forwardSpeed);

        m_previousForwardSpeed = m_forwardSpeed;
    }


    public void PitchAndRollInput(float v, float h)
    {
        m_v = v;
        m_h = h;
    }


    /// <summary>
    /// Input for thrust control 
    /// </summary>
    /// <param name="a">The thrust input value, limit to -1 -> +1</param>
    public void ThrustInput(float a)
    {
        m_a = a;
    }


    public float ForwardSpeed
    {
        get { return m_forwardSpeed; }
    }


    public float MaxForwardSpeed
    {
        get { return m_maxForwardSpeed; }
    }
}
