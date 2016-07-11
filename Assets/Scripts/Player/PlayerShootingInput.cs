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
        if (Input.GetAxisRaw("Jump") == 1 && Time.timeScale > 0)
            m_shootingControlScript.Shoot();
    }
}
