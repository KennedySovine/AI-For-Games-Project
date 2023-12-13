// ----------------------------------------------------------------------
// --------------------  AI: AI Class - Pathfinding
// -------------------- David Dorrington, UoB Games, 2023
// ---------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

public class DD_AI_Class : MonoBehaviour
{
    // ---------------------------------------------------------------------
    public Vector3[] wayPointPositions = new Vector3[25];
    DD_GameManager gameManager;
 //  public GameObject wpMarker = new();

    // ---------------------------------------------------------------------
    private void Start()
    {
        gameManager = GetComponent<DD_GameManager>();
    }//----


    // ---------------------------------------------------------------------

    public bool CheckTargetInLineOfSight(Vector3 pStartPos, Vector3 pTargtetPos)
    {
        Vector3 nextPos = Vector3.zero, currentPos = pStartPos;
        bool canSeeTarget = false;
        bool searching = true;

        while (searching)   // Loop until target  == currentPos or tile is not empty
        {
            // Calculate angle to target
            int dX = (int)Mathf.Round(pTargtetPos.x) - (int)Mathf.Round(currentPos.x);
            int dZ = (int)Mathf.Round(pTargtetPos.z) - (int)Mathf.Round(currentPos.z);
            float angle = Mathf.Atan2(dX, dZ);

            // Calculate next Position
            nextPos.x = currentPos.x + Mathf.Round(1.4F * Mathf.Sin(angle));
            nextPos.z = currentPos.z + Mathf.Round(1.4F * Mathf.Cos(angle));

            // Is the next Pos the target?
            if (nextPos.x == pTargtetPos.x && nextPos.z == pTargtetPos.z)
            {
                canSeeTarget = true;
                searching = false; // exit loop                                
            }
            currentPos = nextPos;
        }
        return canSeeTarget;
    }

}//==========
