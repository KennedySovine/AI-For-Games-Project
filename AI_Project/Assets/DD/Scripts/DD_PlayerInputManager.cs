// ----------------------------------------------------------------------
// --------------------  AI: Input Manager
// -------------------- David Dorrington, UoB Games, 2023
// ---------------------------------------------------------------------

using UnityEngine;

public class DD_PlayerInputManager : MonoBehaviour
{
    // ---------------------------------------------------------------------
    // Camera Control
    public GameObject gameCamera;
    public float camMoveSpeed = 10f;
    public float selectionBorderSize = 50;

    // Mouse Select
    public Vector2 leftClickPosition = new(-1, -1);
    public Vector2 rightClickPosition = new(-1, -1);
    public Vector2 leftDownPosition = new(-1, -1);
    public Vector2 leftUpPosition = new(-1, -1);

    public bool mouseLUP = false;
    public bool mouseLDown = false;
    public bool mouseRDown = false;

    public int lastKeyPressed = 0;


    // ---------------------------------------------------------------------
    void Update()
    {
        GetPlayerMouseClick();
        CameraControl();
    }//-----




    // ---------------------------------------------------------------------
    private void CameraControl()
    {
        // Detect input and update camera postion
        float newCamX = gameCamera.transform.position.x - Input.GetAxis("Vertical") * camMoveSpeed * Time.deltaTime;
        float newCamZ = gameCamera.transform.position.z + Input.GetAxis("Horizontal") * camMoveSpeed * Time.deltaTime;
        float newCamY = gameCamera.transform.position.y;


        if (Input.mousePosition.y > 0 && Input.mousePosition.y < selectionBorderSize) newCamZ = gameCamera.transform.position.z - camMoveSpeed * Time.deltaTime; 
        if (Input.mousePosition.x > 0 && Input.mousePosition.x < selectionBorderSize) newCamX = gameCamera.transform.position.x - camMoveSpeed * Time.deltaTime; ;
        if (Input.mousePosition.y < Screen.height && Input.mousePosition.y > Screen.height - selectionBorderSize) newCamZ = gameCamera.transform.position.z + camMoveSpeed * Time.deltaTime; ;
        if (Input.mousePosition.x < Screen.width &&  Input.mousePosition.x > Screen.width - selectionBorderSize) newCamX = gameCamera.transform.position.x + camMoveSpeed * Time.deltaTime; ;

        gameCamera.transform.position = new(newCamX, newCamY, newCamZ);

        // Zoom by adjusting Field of view with mouse wheel
        if (Input.mouseScrollDelta.y < 0 && gameCamera.GetComponent<Camera>().fieldOfView < 90)
            gameCamera.GetComponent<Camera>().fieldOfView++;

        if (Input.mouseScrollDelta.y > 0 && gameCamera.GetComponent<Camera>().fieldOfView > 10)
            gameCamera.GetComponent<Camera>().fieldOfView--;
    }//---

    // ================================================================================

    private void GetPlayerMouseClick()
    {

    

        //if the left or right mouse button clicked
        if (Input.GetMouseButtonDown(1))
        {
            // Create Ray cast source and target 
            Ray rayCamToMousePostion = Camera.main.ScreenPointToRay(Input.mousePosition);

            // cast Ray 100m and store data in _rh_hit)
            if (Physics.Raycast(rayCamToMousePostion, out RaycastHit rayHit, 100F))
            {
                rightClickPosition = new((int)rayHit.point.x, (int)rayHit.point.z);
                mouseRDown = true;
                print("RM - " + rightClickPosition);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseLUP = true;
            mouseLDown = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            mouseRDown = false;
        }

    }//------

    public Vector3 getRCP()
    {
        return new Vector3(rightClickPosition.x, 0, rightClickPosition.y);
    }


}//==========
