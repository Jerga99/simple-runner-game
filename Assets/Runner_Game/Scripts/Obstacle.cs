
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float m_MovementSpeed = 5.0f;

    void Update() {
        transform.position += m_MovementSpeed * Time.deltaTime * Vector3.back;
    }
}
