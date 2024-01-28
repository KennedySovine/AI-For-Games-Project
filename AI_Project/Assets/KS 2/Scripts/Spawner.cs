using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] team;
    public Vector3 spawnLocation;
    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
