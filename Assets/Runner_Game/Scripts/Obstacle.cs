
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private int m_Damage;
    [SerializeField]
    private float m_MovementSpeed = 5.0f;
    [SerializeField]
    private float m_LineChangeInterval = 2.0f;
    private float m_LineChangeTimer = 0f;
    private Vector3 m_TargetPosition;

    public int Damage => m_Damage;

    void Update() {
        if (Time.time > m_LineChangeTimer) {
            var randomValue = Random.Range(0, 2.0f);
            m_LineChangeTimer = Time.time + m_LineChangeInterval + randomValue;
            SelectNewLine();
        }
        var speed = m_MovementSpeed * Time.deltaTime;
        transform.position += speed * Vector3.back;

        var targetPosition = new Vector3(
            m_TargetPosition.x,
            m_TargetPosition.y,
            transform.position.z
        );

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed / 5);
    }

    private void SelectNewLine() {
        var xPositions = new float[] { -3.5f, -0.5f, 2.5f };
        var yPositions = new float[] { 0.5f, 3f };

        var xPosition = xPositions[UnityEngine.Random.Range(0, xPositions.Length)];
        var yPosition = yPositions[UnityEngine.Random.Range(0, yPositions.Length)];

        m_TargetPosition = new Vector3(xPosition, yPosition, 0);
    }
}
