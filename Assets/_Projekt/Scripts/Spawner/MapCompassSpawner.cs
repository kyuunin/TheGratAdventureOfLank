using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCompassSpawner : MonoBehaviour, IResettable
{
    public GameObject mapPrefab;

    static bool mapSpawned = false;
    static void SpawnMap()
    {
        var list = GameObject.FindObjectsOfType<MapCompassSpawner>();

        var spawner = list[Random.Range(0, list.Length)];
        Instantiate(spawner.mapPrefab, spawner.transform.position, spawner.transform.rotation, spawner.transform.parent);

        mapSpawned = true;
    }

    private void Start()
    {
        if(!mapSpawned)
            SpawnMap();
    }

    public void Reset()
    {
        mapSpawned = false;
    }
}
