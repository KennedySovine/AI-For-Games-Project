using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionHealth : MonoBehaviour
{

    public Minion unitScript = null;
    private GameObject hpBar = null;
    // private GameObject hitText = null;
    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        unitScript = GetComponentInParent<Minion>();
        int currentLevel = unitScript.level;
        CreateHPBar();
    }

    // ---------------------------------------------------------------------
    void FixedUpdate()
    {
        UpdateHPBar();
    }//-----

    void Update()
    {

    }


    // ---------------------------------------------------------------------
    public void Damage(float damage) // Damage Receiver
    {
        unitScript.health -= damage;

    }//-----


    private void UpdateHPBar()
    {
        float currentHealth = unitScript.health;

        if (currentHealth < 0) return;

        // Resize and colour the bar based on current HP
        hpBar.transform.localScale = new Vector3((currentHealth / maxHealth), 0.1F, 0.1F);

        if (currentHealth > maxHealth / 2)
            hpBar.GetComponent<Renderer>().material.color = Color.green;

        if (currentHealth > maxHealth / 4 && currentHealth < maxHealth / 2)
            hpBar.GetComponent<Renderer>().material.color = Color.yellow;

        if (currentHealth < maxHealth / 4)
            hpBar.GetComponent<Renderer>().material.color = Color.red;

    }//-----


    // ---------------------------------------------------------------------
    private void CreateHPBar()
    {
        hpBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        hpBar.transform.position = new(transform.position.x, 1.5f, transform.position.z);
        hpBar.transform.localScale = new(1, 0.1f, 0.1f);
        hpBar.transform.SetParent(transform);
    }//----
}
