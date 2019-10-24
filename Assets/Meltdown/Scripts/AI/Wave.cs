using UnityEngine;

[CreateAssetMenu(fileName = "AIWave", menuName = "AI/AI Wave System/Create new wave", order = 0)]
public class Wave : ScriptableObject {
    [Tooltip("Spawn batches in wave")]
    public AIBatch[] aiBatches;
}

