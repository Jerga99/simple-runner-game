
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_RotationMultiplier = 1.0f;
    [SerializeField]
    private float m_MoveSpeed = 1.0f;
    [SerializeField]
    private float m_MoveStep;
    [SerializeField]
    private float m_MoveCooldown = 0.1f;
    [Header("Jumping")]
    [SerializeField]
    private float m_Gravity = -9.81f;
    [SerializeField]
    private float m_JumpForce = 10.0f;
    private float[] m_Lanes;
    private float m_TargetPositionX;
    private float m_NextAllowedMovement = 0f;
    private float m_VerticalVelocity;
    private Vector3 m_InitialPosition;
    private bool m_IsGrounded = true;
    private bool m_IsMovingHorizontaly = false;
    private float m_TargetRotationX;
    private float m_TargetRotationY;
    private GameManager m_GameManager;

    void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        float middleX = m_TargetPositionX = transform.position.x;
        m_Lanes = new float[] { middleX - m_MoveStep, middleX, middleX + m_MoveStep };
        m_InitialPosition = transform.position;
    }

    void Update()
    {
        HandleRotation();
        HandleMovement();
        HandleJumping();

        if (Mathf.Approximately(transform.position.x, m_TargetPositionX))
        {
            m_IsMovingHorizontaly = false;
        }

        var newPosition = new Vector3(m_TargetPositionX, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, m_MoveSpeed * Time.deltaTime);
    }

    void HandleRotation()
    {
        float rotationAmount = m_RotationMultiplier * Time.deltaTime;

        if (m_IsMovingHorizontaly)
        {
            m_TargetRotationX = 0f;
            m_TargetRotationY += rotationAmount;
        }
        else
        {
            m_TargetRotationY = 0f;
            m_TargetRotationX += rotationAmount;
        }

        Quaternion targetRotation = Quaternion.Euler(m_TargetRotationX, m_TargetRotationY, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationAmount);
    }

    void HandleMovement()
    {
        if (Time.time >= m_NextAllowedMovement)
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                MoveToLane(-1);
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                MoveToLane(+1);
            }
        }
    }

    void HandleJumping()
    {
        if (m_IsGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            m_VerticalVelocity = m_JumpForce;
            m_IsGrounded = false;
        }
        else if (!m_IsGrounded && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            m_VerticalVelocity = m_Gravity / 3;
        }

        // Applying Gravity
        m_VerticalVelocity += m_Gravity * Time.deltaTime;
        transform.position += new Vector3(0, m_VerticalVelocity * Time.deltaTime, 0);

        // Checking if we are grounded
        if (transform.position.y <= m_InitialPosition.y)
        {
            m_IsGrounded = true;
            m_VerticalVelocity = 0;
            transform.position = new Vector3(transform.position.x, m_InitialPosition.y, transform.position.z);
        }
    }

    void MoveToLane(int direction)
    {
        for (int i = 0; i < m_Lanes.Length; i++)
        {

            if (Mathf.Approximately(m_TargetPositionX, m_Lanes[i]))
            {
                int newIndex = Mathf.Clamp(i + direction, 0, m_Lanes.Length - 1);
                m_TargetPositionX = m_Lanes[newIndex];
                break;
            }
        }

        m_NextAllowedMovement = Time.time + m_MoveCooldown;
        m_IsMovingHorizontaly = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Obstacle>(out var obstacle))
        {
            m_GameManager.OnPlayerGotHit(obstacle);
        }
    }
}
