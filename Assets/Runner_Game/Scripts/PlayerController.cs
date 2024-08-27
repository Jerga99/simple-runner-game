
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_RotationMultiplier = 1.0f;

    void Update()
    {
        float rotationAmount = m_RotationMultiplier * Time.deltaTime;
        transform.Rotate(Vector3.right, rotationAmount);
    }
}
