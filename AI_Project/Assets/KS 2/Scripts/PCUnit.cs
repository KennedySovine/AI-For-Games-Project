using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCUnit : MonoBehaviour
{
    // ---------------------------------------------------------------------
    private DD_GameManager gameManager;
    private KS_Unit unitScript = null;
    public Vector3 playerSetTarget = Vector3.zero;

    private GameObject highlight;
    public bool isSelected = false;

    // check if squad active
    // Check unit is part of squad

    // ---------------------------------------------------------------------
    private void Start()
    {
        // The game manager will be use to access the game board
        gameManager = GameObject.Find("GameManager").GetComponent<DD_GameManager>();
        // Unit Control script
        unitScript = GetComponent<KS_Unit>();
    }//-----


    // ---------------------------------------------------------------------
    private void FixedUpdate()
    {
        StateManagerPlayerControl();
    }//-----



    // ---------------------------------------------------------------------
    private void StateManagerPlayerControl()
    {
        if (!unitScript.isPlayerControlled) return;

        if (Vector3.Distance(unitScript.currentPosition, unitScript.nearestMinionPosition) < unitScript.attackRange)
        {
            if (gameManager.ai.CheckTargetInLineOfSight(unitScript.currentPosition, unitScript.nearestMinionPosition))
            {
                unitScript.unitState = States.farm;
            }
        }

    }//-----
}