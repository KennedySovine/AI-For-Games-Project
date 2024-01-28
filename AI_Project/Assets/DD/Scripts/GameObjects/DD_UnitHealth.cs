// ----------------------------------------------------------------------
// --------------------  AI: Unit Health
// -------------------- David Dorrington, UoB Games, 2023
// ---------------------------------------------------------------------

using UnityEngine;

public class DD_UnitHealth : MonoBehaviour
{
    // ---------------------------------------------------------------------
    public KS_Unit unitScript = null;
    private GameObject hpBar = null;
    // private GameObject hitText = null;
    public float maxHealth;

    [Header("Respawn")]
    private float timeTillRespawn = 0;
    private float respawnTimer;

    // ---------------------------------------------------------------------
    void Start()
    {
        unitScript = GetComponentInParent<KS_Unit>();
        int currentLevel = unitScript.level;
        CreateHPBar();
    }
    //----  


    // ---------------------------------------------------------------------
    void FixedUpdate()
    {
        UpdateHPBar();
        CheckHealthLevel();
    }//-----

    void Update()
    {
        if (!(unitScript.isAlive) && respawnTimer < timeTillRespawn )
        {
            respawnTimer += Time.deltaTime;
        }
        else
        {
            timeTillRespawn = 0;
            unitScript.isAlive = true;
        }
    }



    // ---------------------------------------------------------------------
    private void CheckHealthLevel()
    {

        if (unitScript.health < 0)
        {
            //Start a timer
            timeTillRespawn = (unitScript.level * 2) + 4;
            unitScript.isAlive = false;
            unitScript.currentPosition = unitScript.teamBase.transform.position;


            // Deselect unit
            //if(unitScript.isPlayerControlled) GetComponent<DD_UnitPlayerControl>().isSelected = false;
        }
    }//-----


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


}//==========
