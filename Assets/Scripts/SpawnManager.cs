using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static SpawnManager instance; 
    public Transform[] SpawnPoints;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        foreach(Transform tp in SpawnPoints)
        {
            tp.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Transform GetRandomSpawnPoint()
    {
        return SpawnPoints[Random.Range(0, SpawnPoints.Length)];
    }
}
