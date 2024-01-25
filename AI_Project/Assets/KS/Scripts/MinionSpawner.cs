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
        StartCoroutine(WaitToSpawn());
    }

    private IEnumerator WaitToSpawn()
    {
        while (true)
        {
            for (int i = 0; i < 6; i++)
            {
                Instantiate(minionPrefab, spawnLocation, Quaternion.identity);
                yield return new WaitForSecondsRealtime(1.5F);
            }
            yield return new WaitForSecondsRealtime(15.0F);
        }
    }
}
