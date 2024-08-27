using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform m_MovableContainer;
    [SerializeField]
    private Transform m_SpawnPosition;
    [SerializeField]
    private Transform m_DespawnPosition;
    [SerializeField]
    private float m_BaseSpeed = 1.0f;
    private Transform[] m_Movables;

    void Start()
    {
        int childCount = m_MovableContainer.childCount;
        m_Movables = new Transform[childCount];

        for (int i = 0; i < childCount; i++) {
            m_Movables[i] = m_MovableContainer.GetChild(i);
        }
    }

    void Update()
    {
        foreach (var movable in m_Movables) {
            movable.transform.position = new Vector3(
                movable.transform.position.x,
                movable.transform.position.y,
                movable.transform.position.z - Time.deltaTime * m_BaseSpeed
            );

            if (movable.transform.position.z <= m_DespawnPosition.position.z) {
                movable.transform.position = new Vector3(
                    movable.transform.position.x,
                    movable.transform.position.y,
                    m_SpawnPosition.position.z
                );
            }
        }
    }
}
