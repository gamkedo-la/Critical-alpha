using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ShootingControl))]
public class PlayerShootingInput : MonoBehaviour
{
    private ShootingControl m_shootingControlScript;

	
    void Awake()
    {
        m_shootingControlScript = GetComponent<ShootingControl>();
    }


    void Update()
    {
        float input = Input.GetAxisRaw("Jump");

        m_shootingControlScript.Shoot(input);
    }
}
