// ----------------------------------------------------------------------
// --------------------  AI: Unit Object Class
// -------------------- David Dorrington, UoB Games, 2023
// ---------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// Enums
public enum States { farm, idle, chase, attack, flee, wander, roam, enemy }
public enum Heading { north, south, east, west }

public class KS_Unit : DD_BaseObject
{
    // ---------------------------------------------------------------------
    public DD_GameManager gameManager;
    public DD_PlayerInputManager playerInputManager;
    public DD_AI_Class ai;
    private Vector3 newPosition = Vector3.zero;
    public string team;
    public States unitState = States.idle;
    public bool isPlayerControlled = false;

    // Position Variables
    [Header("Unit Positions")]
    public Vector3 targetPosition = Vector3.zero;
    public GameObject teamBase;
    public Vector3 currentPosition = Vector3.zero;
    public Vector3 goalPosition = Vector3.zero;

    [Header("Movement")]
    public float speed = 3;
    public bool isMoving = false;
    public float fleeRange = 20;
    public float stopRange = 2;
    // Wander vars
    public bool obstacleAhead = false;
    public Vector3 originalRotation;
    public Vector3 WP;

    [Header("EXP")]
    public Vector3 nearestMinionPosition = new(-50, 50, -50);
    private GameObject nearestMinion = null;
    private string enemyMinions;
    private int XP = 0;
    public int level = 3;

    [Header("Combat")]
    public int ammo = 1000;
    public Vector3 nearestEnemyPosition = new(-50, 50, -50);
    public float enemyChaseRange = 20;
    public float attackRange = 10;
    public float attackCoolDown = 1;
    public GameObject attackPF = null;
    private float nextAttackTime = 0;
    private string enemyTeam;

    public GameObject nextStruct;



    // ---------------------------------------------------------------------
    private void Start()
    {
        speed = 2;
        unitState = States.wander;
        gameManager = GameObject.Find("GameManager").GetComponent<DD_GameManager>();
        playerInputManager = GameObject.Find("GameManager").GetComponent<DD_PlayerInputManager>();
        ai = gameManager.GetComponent<DD_AI_Class>();
        nextStruct = gameManager.nextStructure(enemyTeam);
        //Get the unit's team
        team = gameObject.tag;
        // Round off postion to nearest int ands store the current position
        currentPosition = transform.position;

        //Setting base stuff for 
        if (team == ("Red"))
        {
            enemyTeam = "Blue";
            enemyMinions = "BlueMinions";
        }
        else
        {
            enemyTeam = "Red";
            enemyMinions = "RedMinions";
        }

    }//----

    // ---------------------------------------------------------------------
    private void FixedUpdate()
    {

        nextStruct = gameManager.nextStructure(enemyTeam);
        currentPosition = gameObject.transform.position;
        //print(newPosition);
        if (isAlive)
        {
            //findNearest();
            StateManager();
            UnitActions();
        }
    }//---

    // ---------------------------------------------------------------------
    private void UnitActions()
    {
        if (unitState == States.farm) FarmMinions();
        if (unitState == States.attack) AttackStruct();
        if (unitState == States.wander) MoveToStruct();
        if (unitState == States.enemy) AttackEnemy();
        if (unitState == States.chase) ChaseClick();

    }//------

    private void StateManager()
    {
        if (isPlayerControlled) return;

        //Check if enemy minion in range
        if (Vector3.Distance(currentPosition, nearestMinionPosition) < attackRange)
        {
            unitState = States.farm;
        }

        //Check if enemy structure is in range
        else if (Vector3.Distance(currentPosition, nextStruct.transform.position) < attackRange)
        {
            unitState = States.attack;
        }

        //Attack enemy if in range
        if (Vector3.Distance(currentPosition, nearestEnemyPosition) < attackRange)
        {
            unitState = States.enemy;
        }


    }

    private void findNearest()
    {

        //Find nearest enemy
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTeam);
        GameObject nearestEnemy = enemies[0];
        nearestEnemyPosition = nearestEnemy.transform.position;

        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(currentPosition, nearestEnemyPosition) < Vector3.Distance(currentPosition, enemy.transform.position))
            {
                nearestEnemy = enemy;
                nearestEnemyPosition = nearestEnemy.transform.position;
            }
        }


    }

    public void FarmMinions()
    {
        SendAttack(nearestMinionPosition);
    }

    public void AttackEnemy()
    {
        SendAttack(nearestEnemyPosition);
    }


    private void SendAttack(Vector3 target)
    {
        if (!attackPF) return; // no bullet object referenced
        speed = 0;

        if (nextAttackTime < Time.time)
        {
            GameObject unitAttack = Instantiate(attackPF, gameObject.transform);
            transform.LookAt(target);
            unitAttack.transform.SetParent(null);
            unitAttack.GetComponent<DD_Attack>().tag = team;
            unitAttack.transform.position = Vector3.MoveTowards(currentPosition, target, 1);
            nextAttackTime = Time.time + attackCoolDown;
        }
    }//-----

    private void AttackStruct()
    {
        speed = 0;
        //If its not alive
        if (!nextStruct.GetComponent<Structures>().isAlive)
        {
            nextStruct = gameManager.nextStructure(enemyTeam);
            return;
        }

        SendAttack(nextStruct.transform.position);
    }

    private void ChaseClick()
    {
        MoveUnit(playerInputManager.RPC);
    }


    private void MoveToStruct()
    {
        MoveUnit(nextStruct.transform.position);
    }

    public void MoveUnit(Vector3 target)
    {
        Vector3 newTargetPos;
        if (ai.isBlocked(currentPosition, target))
        {
            newTargetPos = ai.nearestWPPosition(currentPosition);
        }
        else
        {
            newTargetPos = new Vector3(target.x, currentPosition.y, target.z);
        }
        //newTargetPos = new Vector3(target.x, currentPosition.y, target.z);
        //print(newTargetPos);
        transform.LookAt(newTargetPos);
        transform.position = Vector3.MoveTowards(currentPosition, newTargetPos, speed * Time.deltaTime);
    }//----

    public int XPtoLevel(int currentLevel)
    {
        switch (currentLevel)
        {
            case 3:
                return 478;
            case 4:
                return 510;
            case 5:
                return 750;
            case 6:
                return 780;
            default:
                return 100 + XPtoLevel(currentLevel - 1);
        }
    }

}//==========