using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public GameObject minionPrefab;
    private Vector3 spawnLocation;
    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = gameObject.transform.position;
        InvokeRepeating("spawnMinions", 10f, 15f);
    }

    void spawnMinions()
    {
        Instantiate(minionPrefab, spawnLocation, Quaternion.identity);
    }
}
