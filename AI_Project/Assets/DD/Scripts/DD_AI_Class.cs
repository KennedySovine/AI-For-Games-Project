// ----------------------------------------------------------------------
// --------------------  AI: AI Class - Pathfinding
// -------------------- David Dorrington, UoB Games, 2023
// ---------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

public class DD_AI_Class : MonoBehaviour
{
    // ---------------------------------------------------------------------
    public GameObject[] movementWaypoints;
    DD_GameManager gameManager;
    //  public GameObject wpMarker = new();

    // ---------------------------------------------------------------------
    private void Start()
    {
        gameManager = GetComponent<DD_GameManager>();
        movementWaypoints = gameManager.movementWaypoints;
    }//----


    // ---------------------------------------------------------------------

    public Vector3 nearestWPPosition(Vector3 currentPos)
    {
        Vector3 nearestWP = movementWaypoints[0].transform.position;

        foreach (GameObject WP in movementWaypoints)
        {
            if (Vector3.Distance(currentPos, nearestWP) < Vector3.Distance(currentPos, WP.transform.position))
            {
                nearestWP = WP.transform.position;
            }
        }
        return nearestWP;
    }

    /*public bool isBlocked(Vector3 currentPos, Vector3 target)
    {
        if (Physics.Linecast(currentPos, target))
        {
            return true;
        }
        return false;
    }*/

    public bool isBlocked(GameObject obj)
    {
        RaycastHit hit;
        Vector3 rad = obj.GetComponent<Renderer>().bounds.size;
        float radius = rad.x / 2;

        if (Physics.SphereCast(obj.transform.position, radius , transform.forward, out hit, 5)){
            print(hit);
            return true;
        }
        return false;
    }

    /*public Vector3 WPMove(Vector3 currentPos)
    {

    }*/

}//==========
