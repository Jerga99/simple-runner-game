
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
    private float[] m_Lanes;
    private float m_TargetPositionX;
    private float m_NextAllowedMovement = 0f;

    void Start()
    {
        float middleX = m_TargetPositionX = transform.position.x;
        m_Lanes = new float[] { middleX - m_MoveStep, middleX, middleX + m_MoveStep };
    }

    void Update()
    {
        HandleRotation();
        HandleMovement();

        var newPosition = new Vector3(m_TargetPositionX, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, m_MoveSpeed * Time.deltaTime);
    }

    void HandleRotation()
    {
        float rotationAmount = m_RotationMultiplier * Time.deltaTime;
        transform.Rotate(Vector3.right, rotationAmount);
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
    }
}
