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
    [SerializeField] Vector2 m_pitchAngleMinMax = new Vector2(-45f, 45f); 
    [SerializeField] Vector2 m_bankAngleMinMaxForPitching = new Vector2(-45f, 45f);
    [SerializeField] Vector2 m_altitudeMinMax = new Vector2(100f, 1000f);

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
    private float m_dotThisForwardToPlayer;
    private float m_dotThisUpToPlayer;
    private float m_dotThisRightToPlayer;
    private float m_dotUpToPlayer; 
    private float m_dotThisUpToUp;
    private float m_dotThisRightToUp;
    private float m_forwardAngleToPlayer;
    private float m_pitchAngleToPlayer;
    private float m_pitchAngle;
    private float m_bankAngle;
    private float m_bankMagnitude;
    private float m_bankAngleToAimFor;

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

            CheckAltitude();

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
        if (m_player == null)
        {
            m_state = State.Patrol;
            return;
        }

        m_playerDirection = m_player.position - transform.position;
        var playerDirectionOnGround = new Vector2(m_playerDirection.x, m_playerDirection.z);
        var playerDirectionVertical = new Vector2(playerDirectionOnGround.magnitude, m_playerDirection.y);

        var playerDirectionNormalized = m_playerDirection.normalized;

        m_dotThisForwardToPlayer = Vector3.Dot(transform.forward, playerDirectionNormalized);
        m_dotThisUpToPlayer = Vector3.Dot(transform.up, playerDirectionNormalized);
        m_dotThisRightToPlayer = Vector3.Dot(transform.right, playerDirectionNormalized);
        m_dotThisUpToUp = Vector3.Dot(transform.up, Vector3.up);
        m_dotThisRightToUp = Vector3.Dot(transform.right, Vector3.up);
        m_dotUpToPlayer = Vector3.Dot(Vector3.up, playerDirectionNormalized);

        var forwardOnGround = new Vector2(transform.forward.x, transform.forward.z);

        float angleForwards = Mathf.Atan2(forwardOnGround.x, forwardOnGround.y);
        float anglePlayer = Mathf.Atan2(playerDirectionOnGround.x, playerDirectionOnGround.y);

        m_forwardAngleToPlayer = StandardiseAngle(Mathf.Rad2Deg * (anglePlayer - angleForwards));

        m_bankAngle = StandardiseAngle(transform.rotation.eulerAngles.z);

        if (m_bankAngle > 180f)
            m_bankAngle -= 360f;

        m_bankAngle = -m_bankAngle;

        float playerVerticalAngle = Mathf.Atan2(playerDirectionVertical.y, playerDirectionVertical.x);
        m_pitchAngle = Mathf.Atan2(transform.forward.y, forwardOnGround.magnitude);

        m_pitchAngleToPlayer = Mathf.Rad2Deg * (playerVerticalAngle - m_pitchAngle);
        m_pitchAngle *= -Mathf.Rad2Deg;
    }


    private static float StandardiseAngle(float angle)
    {
        float newAngle = (360f + angle) % 360f;

        if (newAngle > 180f)
            newAngle -= 360f;

        return newAngle;
    }


    private void CheckPlayerRange()
    {
        float range = m_playerDirection.magnitude;

        m_playerInRange = range <= m_playerInRangeAttackThreshold;

        if (!m_playerInRange)
            m_state = State.Patrol;
        else
            m_state = m_dotThisForwardToPlayer > m_evadeMaxDotThreshold ? State.Chase : State.Evade;
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
        BankAngleToAimFor(m_forwardAngleToPlayer);
        SetHorizontal();
        PitchToAimAtPlayer();
    }


    private void BankAngleToAimFor(float angleToTurn)
    {
        float maxAngleToTurn = Mathf.Min(90f, Mathf.Abs(2f * angleToTurn));

        m_bankAngleToAimFor = Mathf.Sign(angleToTurn) * maxAngleToTurn;
    }


    private void SetHorizontal()
    {
        float bankAngleToChange = m_bankAngleToAimFor - m_bankAngle;

        m_h = bankAngleToChange / m_flyingControlScript.turnRate;
        m_h = Mathf.Clamp(m_h, -1f, 1f);
    }


    //private void BankToAimAtPlayer()
    //{
    //    float dotUpAim = Mathf.Abs(m_dotThisUpToPlayer);

    //    m_turnRight = m_dotThisForwardToPlayer > 0 ? true : false;
 
    //    BankTowardsPlayer(); 
    //}


    private void BankTowardsPlayer()
    {
        m_h = m_forwardAngleToPlayer / 90f;
    }


    private void PitchToAimAtPlayer()
    {
        if (m_bankAngle < m_bankAngleMinMaxForPitching.x || m_bankAngle > m_bankAngleMinMaxForPitching.y)
            return;
              
        m_v = -m_pitchAngleToPlayer * 0.1f;

        if (m_v > 0 && m_pitchAngle > m_pitchAngleMinMax.y)
            m_v = 0;
        else if (m_v < 0 && m_pitchAngle < m_pitchAngleMinMax.x)
            m_v = 0;

        m_v = Mathf.Clamp(m_v, -1f, 1f);
    }


    private void CheckAltitude()
    {
        if (transform.position.y < m_altitudeMinMax.x)
        {
            FlattenRoll();
            m_v = -1f;

            if (m_pitchAngle < m_pitchAngleMinMax.x)
                m_v = 0;
        }
        else if (transform.position.y > m_altitudeMinMax.y)
        {
            FlattenRoll();
            m_v = 1f;

            if (m_pitchAngle > m_pitchAngleMinMax.y)
                m_v = 0;
        }

        //if (transform.position.y < m_altitudeMinMax.x || transform.position.y > m_altitudeMinMax.y)
        //{
        //    bool pitchWithinLimits = m_pitchAngle > m_pitchAngleMinMax.x || m_pitchAngle < m_pitchAngleMinMax.y;

        //    FlattenRoll();

        //    if (!pitchWithinLimits || m_bankAngle < m_bankAngleMinMaxForPitching.x || m_bankAngle > m_bankAngleMinMaxForPitching.y)
        //    {
        //        print("Banked too much to pitch");
        //        m_v = 0;
        //    }
        //}
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
        if (m_dotThisRightToUp > 0)
            m_bankMagnitude = !m_turnRight ? m_dotThisUpToUp : 2f - m_dotThisUpToUp;
        else
            m_bankMagnitude = m_turnRight ? m_dotThisUpToUp : 2f - m_dotThisUpToUp;
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
        BankAngleToAimFor(0);
        SetHorizontal();
    }


    private enum State
    {
        Patrol = 0,
        Chase = 1,
        Evade = 2,
        Flee = 3,
    }
}
