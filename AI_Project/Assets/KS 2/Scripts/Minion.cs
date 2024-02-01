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
    public float speed = 2F;
    public bool isMoving = false;
    public GameObject[] waypoints;
    private int waypointNum;
    private GameObject pathing;
    private Vector3 originalRotation;

    [Header("EXP")]
    public Vector3 nearestMinionPosition = new(-50, 50, -50);
    private GameObject nearestMinion = null;
    private string enemyMinions;
    private GameObject[] minions;

    [Header("Combat")]
    public int ammo = 1000;
    public Vector3 nearestEnemyPosition = new(-50, 50, -50);
    public float attackRange = 5;
    public float attackCoolDown = 1;
    public GameObject attackPF = null;
    private float nextAttackTime = 0;
    private string enemyTeam;
    public int damageReduction;

    public GameObject nextStruct;


    // ---------------------------------------------------------------------
    private void Start()
    {
        speed = 1.5F;
        gameManager = GameObject.Find("GameManager").GetComponent<DD_GameManager>();
        waypoints = new GameObject[4];
        waypointNum = 0;
        //Get the unit's team
        team = gameObject.tag;
        // Round off postion to nearest int ands store the current position
        currentPosition = transform.position;
        isAlive = true;

        nextStruct = gameManager.nextStructure(enemyTeam); //Get the first structure they must move to

        unitState = States.roam;
        //Setting base stuff for 
        if (team == ("RedMinion"))
        {
            enemyTeam = "Blue";
            enemyMinions = "BlueMinion";
            pathing = GameObject.Find("RMPathing");
            nearestMinionPosition = GameObject.Find("BMP4").transform.position;
        }
        else
        {
            enemyTeam = "Red";
            enemyMinions = "RedMinion";
            pathing = GameObject.Find("BMPathing");
            nearestMinionPosition = GameObject.Find("RMP4").transform.position;
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
        nextStruct = gameManager.nextStructure(enemyTeam);
        currentPosition = transform.position;
        nearestMinionPosition = gameManager.findNearestMinion(currentPosition, enemyTeam);
        StateManager();
        UnitActions();
    }

    private void UnitActions()
    {
        if (unitState == States.farm) FarmMinions();
        if (unitState == States.attack) AttackStruct();
        if (unitState == States.roam) WaypointMove();
        if (unitState == States.wander) MoveToStruct();

    }//------

    private void StateManager()
    {

        //Check if enemy minion in range
        if (Vector3.Distance(currentPosition, nearestMinionPosition) < attackRange)
        {
            Debug.Log("Here");
            unitState = States.farm;
        }

        //Check if enemy structure is in range
        else if (Vector3.Distance(currentPosition, nextStruct.transform.position) < attackRange)
        {
            unitState = States.attack;
        }

        else if (waypointNum < 4)
        {
            return;
        }

        else
        {
            unitState = States.wander;
        }


    }

    public void FarmMinions()
    {
        speed = 0;
        SendAttack(nearestMinionPosition);
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
            unitAttack.GetComponent<Attack_Minion>().range = attackRange;
            //unitAttack.GetComponent<Attack_Minion>().tag = team;
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
        //Debug.Log(nextStruct.transform.position);
        SendAttack(nextStruct.transform.position);
    }

    private void WaypointMove()
    {
        //Debug.Log(targetPosition);
        GameObject nearestWaypoint = waypoints[waypointNum];
        if (Vector3.Distance(currentPosition, nearestWaypoint.transform.position) < 0.1f)
        {

            if (waypointNum < 3)
            {
                nearestWaypoint = waypoints[waypointNum + 1];
                targetPosition = nearestWaypoint.transform.position;
                waypointNum++;
            }
            else
            {
                GameObject nextStruct = gameManager.nextStructure(enemyTeam);
                targetPosition = nextStruct.transform.position;

            }
        }
        MoveUnit(targetPosition);

    }

    private void MoveToStruct()
    {
        MoveUnit(nextStruct.transform.position);
    }

    private bool isBlocked(Vector3 target)
    {
        if (Physics.Linecast(currentPosition, target))
        {
            return true;
        }
        return false;
    }

    private IEnumerator moveAround(Vector3 target)
    {
        while (isBlocked(target))
        {
            transform.Rotate(new Vector3(0, 1, 0));
        }
        gameObject.transform.position = transform.forward * speed;
        transform.Rotate(originalRotation);
        yield return new WaitForSecondsRealtime(2);
        transform.LookAt(target);
    }


    private void MoveUnit(Vector3 target)
    {
        // If object is blocked, move around it
        /*while (isBlocked(target))
        {
            moveAround(target);
        }*/
        Vector3 newTargetPos = new Vector3(target.x, currentPosition.y, target.z);
        transform.LookAt(newTargetPos);
        transform.position = Vector3.MoveTowards(currentPosition, newTargetPos, speed * Time.deltaTime);
    }//----
}