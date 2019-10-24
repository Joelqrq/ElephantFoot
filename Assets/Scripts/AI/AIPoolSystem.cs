using System.Collections.Generic;
using UnityEngine;

public class AIPoolSystem : MonoBehaviour {

    [SerializeField] private AIType ai = null;
    [SerializeField] private int defaultSpawns = 10;
    private int incrementAmount = 5;
    [SerializeField] private AISpawnSystem aiSpawnSystem = null;
    private Dictionary<int, Queue<AI>> poolList;

    private void Start() {
        if (aiSpawnSystem == null)
            aiSpawnSystem = GetComponent<AISpawnSystem>();

        //Initialize pool list
        poolList = new Dictionary<int, Queue<AI>>(ai.ais.Length);
        for (int i = 0; i < ai.ais.Length; i++) {
            poolList.Add(i, new Queue<AI>());
            IncreasePool(i, defaultSpawns);
        }
    }

    private void IncreasePool(int poolID, int amount) {
        for (int i = 0; i < amount; i++) {
            AI tempAI = Instantiate(ai.ais[poolID]);
            tempAI.ReturnToPoolEvent += ReturnToPool;
            tempAI.gameObject.SetActive(false);
            poolList[poolID].Enqueue(tempAI);
        }
        //Debug.Log("Pool has increased by " + amount);
    }

    public AI GetAIPool(int poolID) {
        if (poolList[poolID].Count == 0)
            IncreasePool(poolID, incrementAmount);

        AI ai = poolList[poolID].Dequeue();
        //Debug.Log("Retrieved " + ai.name + " from pool.");
        return ai;
    }

    public void ReturnToPool(AI ai) {

        for (int i = 0; i < this.ai.ais.Length; i++) {
            if (ai.GetData.Name == this.ai.ais[i].GetData.Name) {
                ai.gameObject.SetActive(false);
                poolList[i].Enqueue(ai);
                return;
            }
        }

        Debug.LogError("AI does not belong to any pool! Got leaked!");
    }
}
