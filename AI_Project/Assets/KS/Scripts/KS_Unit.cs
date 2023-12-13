// ----------------------------------------------------------------------
// --------------------  AI: Unit Object Class
// -------------------- David Dorrington, UoB Games, 2023
// ---------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

// Enums
public enum States { farm, idle, chase, attack, flee, wander, roam }
public enum Heading { north, south, east, west}

public class KS_Unit : DD_BaseObject
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

    [Header("Movement")]
    public float speed = 1;
    public bool isMoving = false;
    public float fleeRange = 20;
    public float stopRange = 2;
    // Wander vars
    public bool obstacleAhead = false;

    [Header("EXP")]
    public Vector3 nearestMinionPosition = new(-50, 50, -50);
    private GameObject nearestMinion = null;
    private string enemyMinions;
    private int XP = 0;
    public int level = 3;

    [Header("Combat")]
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


        if (!isPlayerControlled)
            unitState = States.wander;
        else
            unitState = States.idle;

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
        if (isAlive)
        {
            if (!isMoving)
            {
                StateManager();
                UnitActions();
            }
            MoveUnit();
        }
    }//---

    // ---------------------------------------------------------------------
    private void UnitActions()
    {
        if (unitState == States.chase) ChaseDirect(false);
        if (unitState == States.flee) ChaseDirect(true);
        if (unitState == States.farm) FarmMinions();
        if (unitState == States.attack) AttackEnemy();

    }//------


    // ---------------------------------------------------------------------
    private void StateManager()
    {
        if (isPlayerControlled) return;
        if (isMoving) return;

        // Check Minion in Range 
        if (Vector3.Distance(currentPosition, nearestMinionPosition) < attackRange)
        {
            if (gameManager.ai.CheckTargetInLineOfSight(currentPosition, nearestMinionPosition))
            {
                targetPosition = nearestMinionPosition;
                unitState = States.chase;
            }
        }

        // Check if enemy is close
        if (Vector3.Distance(nearestEnemyPosition, currentPosition) < enemyChaseRange)
        {
            if (gameManager.ai.CheckTargetInLineOfSight(currentPosition, nearestEnemyPosition))
            {
                AttackEnemy();
            }
        }

        // wander if out of range 
        //if (Vector3.Distance(currentPosition, ) > resourceRange)
            unitState = States.wander;

        // is the path blocked and not wandering
        if (unitState != States.wander)
        {
            if (obstacleAhead)
            {
                unitState = States.roam;
                obstacleAhead = false;
            }
        }
    }//---



    //                   ****************   COMBAT ***************************
    // ---------------------------------------------------------------------
    public void AttackEnemy()
    {
        if (!isAlive) return;

        /*if (!nearestEnemy)
        {
            nearestEnemyPosition = new(-50, -50, -50); // out of range
            unitState = States.wander;
            return; // no enemy found
        }
        //Enemy in Chase Range
        if (Vector3.Distance(nearestEnemyPosition, currentPosition) < enemyChaseRange && Vector3.Distance(nearestEnemyPosition, currentPosition) > attackRange)
        {
            targetPosition = nearestEnemyPosition;
            unitState = States.chase;
        }
        else if (Vector3.Distance(nearestEnemyPosition, currentPosition) < attackRange) // in Attack range
        {
            targetPosition = nearestEnemyPosition;
            unitState = States.idle;
            SendAttack();
        }*/

        if (Vector3.Distance(nearestEnemyPosition, currentPosition) < attackRange)
        {
            targetPosition = nearestEnemyPosition;
            //unitState.idle;
            SendAttack();
        }

    }//-----

    public void FarmMinions()
    {
        GameObject[] minions = GameObject.FindGameObjectsWithTag(enemyMinions);
        nearestMinion = minions[0];
        nearestMinionPosition = nearestMinion.transform.position;

        foreach(GameObject minion in minions){
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


    // ---------------------------------------------------------------------



    //                      ****************   RESOURCES  
    // ---------------------------------------------------------------------



    //                      ****************   MOVEMENT   ***************************
    // ---------------------------------------------------------------------
    public void ChaseDirect(bool reverse)
    {
        /*if (!isMoving) // Move to target if unit is not moving
        {

            // Find Straight Line to target  ---------------------------
            float dx = (targetPosition.x - currentPosition.x);
            float dz = (targetPosition.z - currentPosition.z);
            float angle = Mathf.Atan2(dx, dz);

            // use Trig to work out which slot is closest to a straight line to target
            if (!reverse)
            {
                if (Mathf.Abs(dx) > 0.1F) nextPosition.x = currentPosition.x + Mathf.Round(1.4F * Mathf.Sin(angle));
                if (Mathf.Abs(dz) > 0.1F) nextPosition.z = currentPosition.z + Mathf.Round(1.4F * Mathf.Cos(angle));
            }
            else
            {
                if (Mathf.Abs(dx) > 0.1F) nextPosition.x = currentPosition.x - Mathf.Round(1.4F * Mathf.Sin(angle));
                if (Mathf.Abs(dz) > 0.1F) nextPosition.z = currentPosition.z - Mathf.Round(1.4F * Mathf.Cos(angle));
            }

            // Round off next Pos
            nextPosition = new Vector3((int)Mathf.Round(nextPosition.x), 0, (int)Mathf.Round(nextPosition.z));
            int newX = (int)Mathf.Round(nextPosition.x);
            int newZ = (int)Mathf.Round(nextPosition.z);

            // Check if the new postion is on the board and free
        }*/
    }//---



    // ---------------------------------------------------------------------
    private void Wander()
    {
        
    }//-----


    // ---------------------------------------------------------------------
    private void MoveUnit()
    {
        if (isMoving)
        {
            // check distance to new position from  current position
            Vector2 movePos = new(currentPosition.x, currentPosition.z);
            Vector2 curentRealPos = new(transform.position.x, transform.position.z);

            if (Vector2.Distance(movePos, curentRealPos) > 0.1F) // Move Unit to new slot 10cm from centre
            {
                transform.LookAt(currentPosition);
                transform.Translate(0, 0, speed * Time.deltaTime);
            }
            else
            {
                //  print("at target, stopping Moving");
                isMoving = false; // stop moving            
            }
        }
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

    public void setGoal()
    {

    }
}//==========