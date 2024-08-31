
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private int m_Damage;
    [SerializeField]
    private float m_MovementSpeed = 5.0f;

    public int Damage => m_Damage;

    void Update() {
        transform.position += m_MovementSpeed * Time.deltaTime * Vector3.back;
    }
}
