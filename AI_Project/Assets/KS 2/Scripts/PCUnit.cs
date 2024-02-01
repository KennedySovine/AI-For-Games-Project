using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCUnit : MonoBehaviour
{
    // ---------------------------------------------------------------------
    private DD_GameManager gameManager;
    private DD_PlayerInputManager PIM;
    private KS_Unit unitScript = null;
    public Vector3 playerSetTarget = Vector3.zero;

    public bool isSelected = false;

    private Vector3 newPosition;

    // check if squad active
    // Check unit is part of squad

    // ---------------------------------------------------------------------
    private void Start()
    {
        // The game manager will be use to access the game board
        gameManager = GameObject.Find("GameManager").GetComponent<DD_GameManager>();
        PIM = gameManager.GetComponent<DD_PlayerInputManager>();
        //print(PIM);
        // Unit Control script
        unitScript = GetComponent<KS_Unit>();
    }//-----


    // ---------------------------------------------------------------------
    private void FixedUpdate()
    {
        //StateManagerPlayerControl();
    }//-----



    // ---------------------------------------------------------------------
}