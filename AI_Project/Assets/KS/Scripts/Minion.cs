using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : DD_BaseObject

{
    // ---------------------------------------------------------------------
    public DD_GameManager gameManager;
    public string team;
    public States unitState = States.idle;
    public bool isPlayerControlled = false;

    // Position Variables
    [Header("Unit Positions")]
    public Vector3 targetPosition = Vector3.zero;
    public Vector3 basePosition = Vector3.zero;
    public Vector3 currentPosition = Vector3.zero;
    public Vector3 goalPosition = Vector3.zero;
    private GameObject teamInhibitor;

    [Header("Movement")]
    public float speed = 1;
    public bool isMoving = false;

    private GameObject[] waypoints;
    private GameObject pathing;

    [Header("EXP")]
    public Vector3 nearestMinionPosition = new(-50, 50, -50);
    private GameObject nearestMinion = null;
    private string enemyMinions;
    private int XP = 0;
    public int level = 3;

    [Header("Combat")]
    public bool isStronger = false;
    public int ammo = 1000;
    public Vector3 nearestEnemyPosition = new(-50, 50, -50);
    private GameObject nearestEnemy = null;
    public float enemyChaseRange = 20;
    public float attackRange = 10;
    public float attackCoolDown = 1;
    public GameObject attackPF = null;
    private float nextAttackTime = 0;
    private string enemyTeam;


    // ---------------------------------------------------------------------
    private void Start()
    {
        //Get the unit's team
        team = gameObject.tag;
        // Round off postion to nearest int ands store the current position
        currentPosition = transform.position;


        //Setting base stuff for 
        if (team == ("RedMinion"))
        {
            enemyTeam = "Blue";
            enemyMinions = "BlueMinions";
            teamInhibitor = GameObject.Find("RedInhibitor");
            pathing = GameObject.Find("RMPathing");
        }
        else
        {
            enemyTeam = "Red";
            enemyMinions = "RedMinions";
            teamInhibitor = GameObject.Find("BlueInhibitor");
            pathing = GameObject.Find("BMPathing");
        }

        Transform parent = pathing.transform;
        foreach (Transform child in parent)
        {

        }


    }//----

    // Update is called once per frame
    void Update()
    {
        //Check if the inhibitor is destroyed. If so, theyre stronger.


        
    }
}
