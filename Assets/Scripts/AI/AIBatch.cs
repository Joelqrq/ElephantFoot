using UnityEngine;

[CreateAssetMenu(fileName = "AIBatch", menuName = "AI/AI Wave System/Create new batch", order = 1)]
public class AIBatch : ScriptableObject {
    [Tooltip("Spawns in batch")]
    public AISpawn[] aiSpawns;
    [Tooltip("Batch delay in wave")]
    public float delay;
}