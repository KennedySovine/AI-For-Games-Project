using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structures : MonoBehaviour
{
    public bool isAlive = true;
    public bool canRegen;
    public float health;
    public int maxHealth;
    public int healthRegeneration;

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < maxHealth && canRegen)
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

    public void Damage(float damage) // Damage Receiver
    {
        health -= damage;
        if (health <= 0)
        {
            DestroyImmediate(gameObject, true);
        }

    }//-----
}