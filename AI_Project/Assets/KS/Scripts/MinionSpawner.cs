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
        InvokeRepeating("spawnMinions", 2f, 15f);
    }

    //Spawn 6 minions in
    void spawnMinions()
    {
        for (int i = 0; i < 6; i++) {
            Instantiate(minionPrefab, spawnLocation, Quaternion.identity);
            waitToSpawn();
        }
    }

    private IEnumerator waitToSpawn()
    {
        yield return new WaitForSecondsRealtime(2);
    }
}
