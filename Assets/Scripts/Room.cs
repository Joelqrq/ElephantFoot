#pragma warning disable CS0649
using UnityEngine;
using UnityEngine.AI;

public class Room : MonoBehaviour {
    [SerializeField] private Transform spawnPointHolder;
    public Vector3[] SpawnPoints { get; private set; }

    private void Awake() {
        SpawnPoints = new Vector3[spawnPointHolder.childCount];
        for (int i = 0; i < spawnPointHolder.childCount; i++) {
            SpawnPoints[i] = spawnPointHolder.GetChild(i).position;
        }
    }
}
