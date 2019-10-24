#pragma warning disable CS0649
using UnityEngine;

[CreateAssetMenu(fileName = "AI", menuName = "AI/Create new AI", order = 1)]
public class AIData : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private float health;
    [SerializeField] private float damage;
    [SerializeField] private float acceleration;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float range;

    public string Name => name;
    public float Health => health;
    public float Damage => damage;
    public float Acceleration => acceleration;
    public float MoveSpeed => moveSpeed;
    public float RotateSpeed => rotateSpeed;
    public float Range => range;
}

[System.Serializable]
public struct AILevelData {
    private Transform target;
    public Wave[] waves;
    public Transform Target => target;
    public void SetTarget(Transform target) {
        this.target = target;
    }
}

[System.Serializable]
public struct AISpawn {
#if UNITY_EDITOR
    [AIType, Tooltip("AI to spawn.")]
#endif
    public int ai;
    [Tooltip("Amount of AI to spawn.")]
    public int count;
    [Tooltip("Spawn delay in batch")]
    public float delay;
    [Tooltip("Spawn interval in batch")]
    public float interval;
}
