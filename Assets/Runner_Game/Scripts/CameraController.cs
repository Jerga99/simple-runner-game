
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform m_FollowTarget;
    private Vector3 m_Offset;

    void Start() {
        m_Offset = m_FollowTarget.transform.position - transform.position;
    }

    void LateUpdate() {
        transform.position = m_FollowTarget.position - m_Offset;
    }
}
