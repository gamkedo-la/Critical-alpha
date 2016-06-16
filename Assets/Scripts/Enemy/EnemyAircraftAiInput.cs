using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FlyingControl), typeof(EnemyHealth))]
public class EnemyAircraftAiInput : MonoBehaviour
{
    [SerializeField] float m_playerInRangeAttackThreshold = 500f;
    [SerializeField] float m_fleeHealthProportion = 0.3f; 
    [SerializeField] float m_evadeMaxDotThreshold = -0.5f;
    [SerializeField] float m_controlsSensitivity = 0.5f;
    [SerializeField] float m_decisionRate = 0.2f;
    [SerializeField] Vector2 m_evadeChangeTimeMinMax = new Vector2(0.5f, 4f); 

    private WaitForSeconds m_waitTime;

    private FlyingControl m_flyingControlScript;
    private EnemyHealth m_health;
    private Transform m_player;

    private Vector3 m_spawnPoint;
    private Quaternion m_spawnRotation;
    private float m_spawnBankAngle;
    private float m_patrolSpeed;
    private int m_fleeHealth;

    private State m_state = State.Patrol;
    private Vector3 m_playerDirection;
    private bool m_playerInRange;
    private float m_dotForwardToPlayer;
    private float m_dotUpToPlayer;
    private float m_dotRightToPlayer;   
    private float m_dotUp;
    private float m_dotRight;
    private float m_bankAngle;
    private float m_bankMagnitude;

    private bool m_turnRight;
    private float m_evadeChangeTime;
    private float m_timeSinceEvadeChange;

    private float m_v;
    private float m_h;
    private float m_a;


    void Awake()
    {
        m_flyingControlScript = GetComponent<FlyingControl>();
        m_health = GetComponent<EnemyHealth>();
        
        var playerObject = GameObject.FindGameObjectWithTag(Tags.Player);

        if (playerObject != null)
            m_player = playerObject.transform;

        m_waitTime = new WaitForSeconds(m_decisionRate);
    }


    void Start()
    {
        m_spawnPoint = transform.position;
        m_spawnRotation = transform.rotation;
        m_spawnBankAngle = transform.rotation.eulerAngles.z;
        m_fleeHealth = Mathf.RoundToInt(m_health.CurrentHealth * m_fleeHealthProportion);
        m_patrolSpeed = m_flyingControlScript.ForwardSpeed;

        if (m_player != null)
            StartCoroutine(MakeDecisions());
    }


    private IEnumerator MakeDecisions()
    {
        float randomTime = Random.Range(0f, m_decisionRate);
        yield return new WaitForSeconds(randomTime);

        while(true)
        {
            CheckOrientation();
            CheckPlayerRange();
            CheckHealth();

            switch (m_state)
            {
                case (State.Patrol):
                    UpdatePatrol();
                    break;

                case (State.Chase):
                    UpdateChase();
                    break;

                case (State.Evade):
                    UpdateEvade();
                    break;

                case (State.Flee):
                    UpdateFlee();
                    break;
            }

            yield return m_waitTime;
        }
    }


    void Update()
    {
        if (m_player == null)
            return;

        m_flyingControlScript.PitchAndRollInput(m_v * m_controlsSensitivity, m_h * m_controlsSensitivity);
        m_flyingControlScript.ThrustInput(m_a * m_controlsSensitivity);
    }


    private void CheckHealth()
    {
        int health = m_health.CurrentHealth;

        if (health <= m_fleeHealth 
            && m_playerInRange)
            m_state = State.Flee;
    }


    private void CheckOrientation()
    {
        m_playerDirection = m_player.position - transform.position;

        var playerDirectionNormalized = m_playerDirection.normalized;

        m_dotForwardToPlayer = Vector3.Dot(transform.forward, playerDirectionNormalized);
        m_dotUpToPlayer = Vector3.Dot(transform.up, playerDirectionNormalized);
        m_dotRightToPlayer = Vector3.Dot(transform.right, playerDirectionNormalized);
        m_dotUp = Vector3.Dot(transform.up, Vector3.up);
        m_dotRight = Vector3.Dot(transform.right, Vector3.up);

        m_bankAngle = (360f + transform.rotation.eulerAngles.z) % 360f;

        if (m_bankAngle > 180f)
            m_bankAngle -= 360f;
    }


    private void CheckPlayerRange()
    {
        float range = m_playerDirection.magnitude;

        m_playerInRange = range <= m_playerInRangeAttackThreshold;

        if (!m_playerInRange)
            m_state = State.Patrol;
        else
            m_state = m_dotForwardToPlayer > m_evadeMaxDotThreshold ? State.Chase : State.Evade;
    }


    private void UpdatePatrol()
    {
        m_a = Mathf.Clamp(m_patrolSpeed - m_flyingControlScript.ForwardSpeed, -1f, 1f);

        float bankAngle = transform.rotation.eulerAngles.z;

        m_h = Mathf.Clamp((bankAngle - m_spawnBankAngle) * 0.05f, -1f, 1f);

        FlattenPitch();
    }


    private void UpdateChase()
    {
        BankToAimAtPlayer();
        PitchToAimAtPlayer();
    }


    private void BankToAimAtPlayer()
    {
        // TODO: check dot right to player
        FlattenRoll();
    }


    private void PitchToAimAtPlayer()
    {
        // TODO: check dot up to player
        FlattenPitch();
    }


    private void UpdateEvade()
    {
        FlattenPitch();

        m_a = Mathf.Clamp(m_patrolSpeed - m_flyingControlScript.ForwardSpeed, -1f, 1f);

        m_timeSinceEvadeChange += m_decisionRate;

        if (m_timeSinceEvadeChange > m_evadeChangeTime)
        {
            int choice = Random.Range(0, 2);
            m_turnRight = choice > 0;

            m_evadeChangeTime = Random.Range(m_evadeChangeTimeMinMax.x, m_evadeChangeTimeMinMax.y);
            m_timeSinceEvadeChange = 0f;
        }

        Bank();
    }


    private void Bank()
    {  
        if (m_turnRight)
            BankToRight();
        else
            BankToLeft();
    }


    private void CalculateBankMagnitude()
    {
        if (m_dotRight > 0)
            m_bankMagnitude = !m_turnRight ? m_dotUp : 2f - m_dotUp;
        else
            m_bankMagnitude = m_turnRight ? m_dotUp : 2f - m_dotUp;
    }


    private void BankToLeft()
    {
        CalculateBankMagnitude();

        m_h = -m_bankMagnitude;

        m_h = Mathf.Clamp(m_h, -1f, 1f);
    }


    private void BankToRight()
    {
        CalculateBankMagnitude();

        m_h = m_bankMagnitude;

        m_h = Mathf.Clamp(m_h, -1f, 1f);
    }


    private void UpdateFlee()
    {
        if (m_flyingControlScript.ForwardSpeed < m_flyingControlScript.MaxForwardSpeed)
            m_a = 1;
        else
            m_a = 0f;

        FlattenPitch();
        FlattenRoll();
    }


    private void FlattenPitch()
    {
        var forward = transform.forward;
        var forwardOnGround = new Vector3(forward.x, 0f, forward.z);
        float dotForward = Vector3.Dot(forward, forwardOnGround.normalized);
        float dotUp = Vector3.Dot(Vector3.up, forward);
        m_v = dotUp;
    }


    private void FlattenRoll()
    {
        m_h = Mathf.Clamp(m_bankAngle * 0.05f, -1f, 1f);
    }


    private enum State
    {
        Patrol = 0,
        Chase = 1,
        Evade = 2,
        Flee = 3,
    }
}
