using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private SpawnConfigSO m_SpawnConfigSO;
    [SerializeField]
    private Transform m_MovableContainer;
    [SerializeField]
    private Transform m_SpawnPosition;
    [SerializeField]
    private Transform m_DespawnPosition;
    [SerializeField]
    private float m_BaseSpeed = 1.0f;
    private Transform[] m_Movables;
    private List<Obstacle> m_ActiveSpawns = new();

    void Start()
    {
        int childCount = m_MovableContainer.childCount;
        m_Movables = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            m_Movables[i] = m_MovableContainer.GetChild(i);
        }

        m_SpawnConfigSO.Init();
    }

    void Update()
    {
        HandleMovables();
        HandleSpawning();
        GarbageCollector();
    }

    private void HandleMovables()
    {
        foreach (var movable in m_Movables)
        {
            movable.transform.position = new Vector3(
                movable.transform.position.x,
                movable.transform.position.y,
                movable.transform.position.z - Time.deltaTime * m_BaseSpeed
            );

            if (movable.transform.position.z <= m_DespawnPosition.position.z)
            {
                movable.transform.position = new Vector3(
                    movable.transform.position.x,
                    movable.transform.position.y,
                    m_SpawnPosition.position.z
                );
            }
        }
    }

    private void HandleSpawning()
    {
        foreach (var spawnable in m_SpawnConfigSO.Spawnables)
        {
            // var spawn = spawnable.Spawn(ChooseSpawnPosition());
            // if (spawn != null) {
            //     m_ActiveSpawns.Add(spawn);
            // }

            if (spawnable.Spawn(ChooseSpawnPosition(), out var spawn)) {
                m_ActiveSpawns.Add(spawn);
            }

        }
    }

    private void GarbageCollector()
    {
        for (int i = 0; i < m_ActiveSpawns.Count; i++) {
            var spawn = m_ActiveSpawns[i];

            if (spawn.transform.position.z <= m_DespawnPosition.position.z) {
                m_ActiveSpawns.RemoveAt(i);
                Destroy(spawn.gameObject);
            }
        }
    }

    private Vector3 ChooseSpawnPosition()
    {
        var xPositions = new float[] { -3.5f, -0.5f, 2.5f };
        var yPositions = new float[] { 0.5f, 3f };

        var xPosition = xPositions[UnityEngine.Random.Range(0, xPositions.Length)];
        var yPosition = yPositions[UnityEngine.Random.Range(0, yPositions.Length)];

        return new Vector3(xPosition, yPosition, m_SpawnPosition.position.z);
    }
}
