using UnityEngine;

[ExecuteInEditMode, CreateAssetMenu(fileName = "AITypes", menuName = "AI/Create new AITypes", order = 0)]
public class AIType : ScriptableObject {
    public AI[] ais = null;
}
