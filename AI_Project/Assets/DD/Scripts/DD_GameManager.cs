// ----------------------------------------------------------------------
// --------------------  AI Game Manager
// -------------------- David Dorrington, UoB Games, 2023
// ---------------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class DD_GameManager : MonoBehaviour
{
    // ---------------------------------------------------------------------
    // Game Data
    private DD_PlayerInputManager playerInputManager;

    [Header("Unit Targets")]
    public Vector3 playerSetTargetPos = new(-50, 50, -50);

    [Header("Game Objects")]
    public DD_AI_Class ai;


    [Header("Team Infrastructure")]
    public GameObject[] redInfrastructure = new GameObject[6];
    public GameObject[] blueInfrastructure = new GameObject[6];

    [Header("Holders")]
    public GameObject[] redMinions;
    public GameObject[] blueMinions;
    public GameObject[] redTeam = new GameObject[5];
    public GameObject[] blueTeam = new GameObject[5];

    public GameObject[] movementWaypoints;

    // UI
    [Header("Game UI")]
    public Text LeftTextWindow;

    // ---------------------------------------------------------------------
    private void Awake()// Runs before start on other objects
    {
        // Create Array for play area
        playerInputManager = GetComponent<DD_PlayerInputManager>();
        ai = GetComponent<DD_AI_Class>();

    }//------

    // ---------------------------------------------------------------------
    private void Start()
    {
        movementWaypoints = GameObject.FindGameObjectsWithTag("MoveWP");
    }//---

    // ---------------------------------------------------------------------
    private void FixedUpdate() // Capped at 50 FPS
    {
        DisplayGameData();
        ChangeStateOnKeyPress();

        // SetSelectionMarkers();
    }//---

    private void Update()
    {
    }


    // ---------------------------------------------------------------------
    private void DisplayGameData()
    {
    }//-----

    /*private GameObject[] sortStructures(GameObject[] arr)
    {

    }*/


    public GameObject nextStructure(string team)
    {
        //Debug.Log(team);
        if (team == "Red")
        {
            foreach (GameObject structure in redInfrastructure)
            {
                if (structure.GetComponent<Structures>().isAlive)
                {
                    return structure;
                }
            }
        }
        else if (team == "Blue")
        {
            foreach (GameObject structure in blueInfrastructure)
            {
                if (structure.GetComponent<Structures>().isAlive)
                {
                    return structure;
                }
            }
        }
        return null;
    }

    public Vector3 findNearestMinion(Vector3 currentPos, string team)
    {
        if (team == "Blue")
        {
            blueMinions = GameObject.FindGameObjectsWithTag("BlueMinion");
            GameObject nearestMinion = blueMinions[0];
            Vector3 nearestMinionPos = nearestMinion.transform.position;
            foreach (GameObject minion in blueMinions)
            {
                if (Vector3.Distance(currentPos, nearestMinionPos) < Vector3.Distance(currentPos, minion.transform.position))
                {
                    nearestMinion = minion;
                    nearestMinionPos = nearestMinion.transform.position;
                }
            }
            return nearestMinionPos;
        }
        else
        {
            redMinions = GameObject.FindGameObjectsWithTag("RedMinion");
            GameObject nearestMinion = redMinions[0];
            Vector3 nearestMinionPos = nearestMinion.transform.position;
            foreach (GameObject minion in redMinions)
            {
                if (Vector3.Distance(currentPos, nearestMinionPos) < Vector3.Distance(currentPos, minion.transform.position))
                {
                    nearestMinion = minion;
                    nearestMinionPos = nearestMinion.transform.position;
                }
            }
            return nearestMinionPos;

        }
    }





    //  ************************** Older Functions Not Currently Used ****************************
    // ---------------------------------------------------------------------
    private void ChangeStateOnKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach (GameObject go in blueTeam)
            {
                go.GetComponent<KS_Unit>().unitState = States.chase;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (GameObject go in blueTeam)
            {
                go.GetComponent<KS_Unit>().unitState = States.wander;
            }
        }
        /*if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach (GameObject go in activeUnits)
            {
                go.GetComponent<KS_Unit>().unitState = States.wander;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            foreach (GameObject go in activeUnits)
            {
                go.GetComponent<KS_Unit>().unitState = States.chase;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            foreach (GameObject go in activeUnits)
            {
                go.GetComponent<KS_Unit>().unitState = States.flee;
            }
        }*/

    }

}//==========