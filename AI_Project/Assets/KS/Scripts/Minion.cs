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
    public Vector3 currentPosition = Vector3.zero;

    [Header("Movement")]
    public float speed = 1;
    public bool isMoving = false;
    public GameObject[] waypoints;
    private int waypointNum;
    private GameObject pathing;

    [Header("EXP")]
    public Vector3 nearestMinionPosition = new(-50, 50, -50);
    private GameObject nearestMinion = null;
    private string enemyMinions;

    [Header("Combat")]
    public int ammo = 1000;
    public Vector3 nearestEnemyPosition = new(-50, 50, -50);
    private GameObject nearestEnemy = null;
    public float attackRange = 10;
    public float attackCoolDown = 1;
    public GameObject attackPF = null;
    private float nextAttackTime = 0;
    private string enemyTeam;
    private string myTeam;
    private GameObject[] teamMembers;
    public int damageReduction;


    // ---------------------------------------------------------------------
    private void Start()
    {
        waypoints = new GameObject[4];
        waypointNum = 0;
        //Get the unit's team
        team = gameObject.tag;
        // Round off postion to nearest int ands store the current position
        currentPosition = transform.position;
        isAlive = true;

        unitState = States.roam;
        //Setting base stuff for 
        if (team == ("RedMinion"))
        {
            enemyTeam = "Blue";
            myTeam = "Red";
            enemyMinions = "BlueMinions";
            pathing = GameObject.Find("RMPathing");
        }
        else
        {
            enemyTeam = "Red";
            myTeam = "Blue";
            enemyMinions = "RedMinions";
            pathing = GameObject.Find("BMPathing");
        }

        Transform parent = pathing.transform;
        int i = 0;
        foreach (Transform child in parent)
        {
            waypoints[i] = child.gameObject;
            i++;
        }


    }//----

    private void FixedUpdate()
    {
        currentPosition = transform.position;
        if (isAlive)
        {

                StateManager();
                UnitActions();
            
        }
    }

    private void UnitActions()
    {
        if (unitState == States.farm) FarmMinions();
        //if (unitState == States.attack) AttackEnemy();
        if (unitState == States.roam) Roam();

    }//------

    private void StateManager()
    {
        if (waypointNum != 3) return;
        //Check if enemy minion in range
        if (Vector3.Distance(currentPosition, nearestMinionPosition) < attackRange)
        {
            if (gameManager.ai.CheckTargetInLineOfSight(currentPosition, nearestMinionPosition))
            {
                unitState = States.farm;
            }
        }

        //Check if enemy structure is in range
        /*else if (gameManager.ai.CheckTargetInLineOfSight(currentPosition, targetPosition))
        {
            unitState = States.attack;
        }*/

        else
        {
            unitState = States.roam;
        }
    }

    public void FarmMinions()
    {
        GameObject[] minions = GameObject.FindGameObjectsWithTag(enemyMinions);
        nearestMinion = minions[0];
        nearestMinionPosition = nearestMinion.transform.position;

        foreach (GameObject minion in minions)
        {
            if (Vector3.Distance(currentPosition, nearestMinionPosition) < Vector3.Distance(currentPosition, minion.transform.position))
            {
                nearestMinion = minion;
                nearestMinionPosition = nearestMinion.transform.position;
            }
        }

        if (Vector3.Distance(currentPosition, nearestMinionPosition) < attackRange)
        {
            nearestEnemyPosition = nearestMinion.transform.position;
            SendAttack();
        }
    }

    private void SendAttack()
    {
        if (!attackPF) return; // no bullet object referenced

        if (nextAttackTime < Time.time)
        {
            transform.LookAt(nearestEnemyPosition);
            GameObject unitAttack = Instantiate(attackPF, gameObject.transform);
            unitAttack.transform.SetParent(null);
            unitAttack.GetComponent<DD_Attack>().tag = team;
            nextAttackTime = Time.time + attackCoolDown;
        }
    }//-----

    private void Roam()
    {
        if (!isMoving)
        {

            //Debug.Log(targetPosition);
            GameObject nearestWaypoint = waypoints[waypointNum];

            if (waypointNum !<= 3)
            {
                nearestWaypoint = waypoints[waypointNum + 1];
                targetPosition = nearestWaypoint.transform.position;
            }
            else
            {
                targetPosition = gameManager.nextStructure(enemyTeam).transform.position;
            }
            MoveUnit();
        }
        
    }

    private void MoveUnit()
    {
        transform.LookAt(targetPosition);
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
        isMoving = false;
    }//----
}
