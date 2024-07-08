using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    public static int mapIndex = 0;
    public Transform Maps; 

    public Transform[] FutureCity;
    public Transform[] Sandland;
    public Transform[] Warehouse;
    public Transform[] Docks;



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DeactivateAllMaps();
        DeactivateAllSpawnPoints();
    }
    private void DeactivateAllMaps()
    {
        int index = 0;
        foreach (Transform map in Maps)
        {
            map.gameObject.SetActive(index == mapIndex);
            index++;
        }
    }

 

    private void DeactivateAllSpawnPoints()
    {
        foreach (Transform tp in Sandland)
        {
            tp.gameObject.SetActive(false);
        }
        foreach (Transform tp in FutureCity)
        {
            tp.gameObject.SetActive(false);
        }
        foreach (Transform tp in Docks)
        {
            tp.gameObject.SetActive(false);
        }
        foreach (Transform tp in Warehouse)
        {
            tp.gameObject.SetActive(false);
        }
    }

    public Transform GetRandomSpawnPoint()
    {
        if (mapIndex == 0)
        {
            return Sandland[Random.Range(0, Sandland.Length)];
        }
        else if(mapIndex==1)
        {
            return FutureCity[Random.Range(0, FutureCity.Length)];
        }
        else if (mapIndex == 3)
        {
            return Warehouse[Random.Range(0, Warehouse.Length)];
        }
        else
        {
            return Docks[Random.Range(0, Docks.Length)];
        }
    }
}
