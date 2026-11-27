using System;
using UnityEngine;

public class InvokeEventSpawnEnemy : MonoBehaviour
{
    public int IDEnemy;
    public int SpawnCount;
    public float RMinSpawn;
    public float RMaxSpawn;
    [HideInInspector] public int RandomSeed;

    // first for id, second for SpawnCount, 3rd for RMinSpawn, 4th for RMaxSpawn, 5th for RandomSeed
    public static event Action<int,int,float,float,int> InvokeEvent_SpawnEnemy;
    public void OnTrigger()
    {
        Debug.Log("ButtonPressed");
        RandomSeed = UnityEngine.Random.Range(1, 10000);
        InvokeEvent_SpawnEnemy?.Invoke(IDEnemy,SpawnCount,RMinSpawn,RMaxSpawn,RandomSeed);
    }
}
