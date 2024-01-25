using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structures : MonoBehaviour
{
    public bool isAlive = false;
    public int health;
    public int maxHealth;
    public int healthRegeneration;

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < maxHealth)
        {
            InvokeRepeating("regenHealth", 0, 1f);
        }
        else
        {
            CancelInvoke();
        }
        
    }

    void regenHealth()
    {
        health += healthRegeneration;
    }
}
