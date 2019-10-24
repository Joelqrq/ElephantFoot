#pragma warning disable CS0649
using System.Collections;
using UnityEngine;

public class AISpawnSystem : MonoBehaviour {
    [SerializeField] private CharacterMovement player;
    [Header("Collectible")]
    [SerializeField] private Collectible[] collectibles;
    [SerializeField] private float collectibleInterval = 5f;
    [Header("Level & Wave")]
    [SerializeField] private AIPoolSystem aiPoolSystem = null;
    [SerializeField] private float waveInterval;
    [SerializeField] private GameObject[] levels;
    [SerializeField] private AILevelData levelData = new AILevelData();
    public AILevelData GetLevelData => levelData;
    private Room[] rooms;
    private Coroutine waveCoroutine;

    private void Start() {
        if (aiPoolSystem == null)
            aiPoolSystem = GetComponent<AIPoolSystem>();

        StartLevel();
    }

    /// <summary> Call this to start level. </summary>
    public void StartLevel() {
        rooms = Instantiate(levels[Random.Range(0, levels.Length)]).GetComponentsInChildren<Room>();
        int roomNo = Random.Range(0, rooms.Length);
        while (rooms[roomNo].SpawnPoints.Length == 0) {
            roomNo = Random.Range(0, rooms.Length);
        }
        Transform tempPlayer = Instantiate(player, rooms[roomNo].SpawnPoints[Random.Range(0, rooms[roomNo].SpawnPoints.Length)], Quaternion.identity).transform;
        //Set level target for AIs
        levelData.SetTarget(tempPlayer);
        //Set camera target
        Camera.main.GetComponent<TopDownCamera>().target = tempPlayer;

        waveCoroutine = StartCoroutine(StartWave());
        StartCoroutine(SpawnCollectible());
    }

    private IEnumerator StartWave() {
        Debug.Log("Wave has started!");
        AIBatch[] batches = levelData.waves[Random.Range(0, levelData.waves.Length)].aiBatches;
        for (int i = 0; i < batches.Length; i++) {
            StartCoroutine(SpawnBatch(batches[i]));
        }

        yield return new WaitForSeconds(waveInterval);
        waveCoroutine = StartCoroutine(StartWave());
    }

    private IEnumerator SpawnBatch(AIBatch batch) {
        yield return new WaitForSeconds(batch.delay);
        //Debug.Log(batch.name + " has started!");
        AISpawn[] spawns = batch.aiSpawns;
        for (int i = 0; i < spawns.Length; i++) {
            StartCoroutine(SpawnAI(spawns[i]));
        }
    }

    private IEnumerator SpawnAI(AISpawn spawn) {
        yield return new WaitForSeconds(spawn.delay);
        StartCoroutine(SpawnInterval(spawn));
    }

    private IEnumerator SpawnInterval(AISpawn spawn) {
        int currentSpawn = 0;
        int roomNo = 0;
        Vector3 randomPos;
        while (currentSpawn < spawn.count) {
            AI ai = aiPoolSystem.GetAIPool(spawn.ai);
            roomNo = Random.Range(0, rooms.Length);
            while (rooms[roomNo].SpawnPoints.Length == 0) {
                roomNo = Random.Range(0, rooms.Length);
            }
            randomPos = rooms[roomNo].SpawnPoints[Random.Range(0, rooms[roomNo].SpawnPoints.Length)];
            ai.Spawn(randomPos, levelData.Target);
            ai.gameObject.SetActive(true);
            currentSpawn++;
            //Debug.Log(ai.name + " has spawned!");
            yield return new WaitForSeconds(spawn.interval);
        }
    }

    private IEnumerator SpawnCollectible() {
        //Spawn Collectible
        int roomNo = Random.Range(0, rooms.Length);
        while (rooms[roomNo].SpawnPoints.Length == 0) {
            roomNo = Random.Range(0, rooms.Length);
        }
        Vector3 randomPos = rooms[roomNo].SpawnPoints[Random.Range(0, rooms[roomNo].SpawnPoints.Length)];
        Instantiate(collectibles[Random.Range(0, collectibles.Length)], randomPos, Quaternion.identity);
        yield return new WaitForSeconds(collectibleInterval);
        StartCoroutine(SpawnCollectible());
    }
}
