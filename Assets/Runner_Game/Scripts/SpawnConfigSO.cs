
using System;
using UnityEngine;

[Serializable]
class Spawnable
{
    [SerializeField]
    private Obstacle m_ObstaclePrefab;
    [SerializeField]
    private float m_SpawnInterval = 3.0f;
    private float m_SpawnTimer = 0f;

    public void Init()
    {
        m_SpawnTimer = m_SpawnInterval;
    }

    public Obstacle Spawn(Vector3 position)
    {
        if (Time.time > m_SpawnTimer)
        {
            m_SpawnTimer = Time.time + m_SpawnInterval;
            return UnityEngine.Object.Instantiate(m_ObstaclePrefab, position, Quaternion.identity);
        }

        return null;
    }

    public bool Spawn(Vector3 position, out Obstacle spawn)
    {
        if (Time.time > m_SpawnTimer)
        {
            m_SpawnTimer = Time.time + m_SpawnInterval;
            spawn = UnityEngine.Object.Instantiate(m_ObstaclePrefab, position, Quaternion.identity);
            return true;
        }

        spawn = null;
        return false;
    }

}

[CreateAssetMenu(menuName = "Game/New Spawn Config", fileName = "SpawnConfigSO")]
class SpawnConfigSO : ScriptableObject
{
    [SerializeField]
    private Spawnable[] m_Spawnables;

    public Spawnable[] Spawnables => m_Spawnables;

    private void OnValidate()
    {
        Init();
    }

    public void Init()
    {
        foreach (var spawnable in m_Spawnables)
        {
            spawnable.Init();
        }
    }
}
