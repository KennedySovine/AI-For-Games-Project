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


    // Lists of Active Objects
    private readonly List<GameObject> activeTeams = new();
    public List<GameObject> activeUnits = new();
    public List<GameObject> activeResources = new();
    public List<GameObject> selectedPlayerUnits = new();
    private bool playerMarkerActive = false;

    [Header("Team Infrastructure")]
    public GameObject[] redInfrastructure = new GameObject[6];
    public GameObject[] blueInfrastructure = new GameObject[6];

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

        SetInfrastructure();
    }//---

    // ---------------------------------------------------------------------
    private void FixedUpdate() // Capped at 50 FPS
    {
        DisplayGameData();
        // SetSelectionMarkers();
    }//---

    private void Update()
    {
    }

    // ---------------------------------------------------------------------
    private void SetInfrastructure()
    {
        redInfrastructure = GameObject.FindGameObjectsWithTag("Red");
        blueInfrastructure = GameObject.FindGameObjectsWithTag("Blue");

    }//------


    // ---------------------------------------------------------------------
    private void DisplayGameData()
    {
    }//-----


    public GameObject nextStructure (string team)
    {
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





    //  ************************** Older Functions Not Currently Used ****************************
    // ---------------------------------------------------------------------
    private void ChangeStateOnKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach (GameObject go in activeUnits)
            {
                go.GetComponent<KS_Unit>().unitState = States.idle;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (GameObject go in activeUnits)
            {
                go.GetComponent<KS_Unit>().unitState = States.roam;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
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
        }

    }//---

}//==========