using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;

public class MobSpawner : MonoBehaviour
{
    public int countMobInLevel = 3;
    [SerializeField] public GameObject mob;
    [SerializeField]public List<GameObject> spawnPoints;
    [SerializeField] public LookAtMobs playerRotator;
    void Awake()
    {
        
        StartCoroutine(SpawnMobWithPauseAndStartBehaviourNtimes(countMobInLevel));
    }

    IEnumerator SpawnMobWithPauseAndStartBehaviourNtimes(int N)
    {
        for (int i = 0; i < N; i++)
        {
            var newAgent = Instantiate(mob, spawnPoints[i].transform.position, Quaternion.identity);
            newAgent.AddComponent<BehaviourTreeOwner>().StartBehaviour();
            yield return new WaitForSeconds(2f);
        }
    }

}