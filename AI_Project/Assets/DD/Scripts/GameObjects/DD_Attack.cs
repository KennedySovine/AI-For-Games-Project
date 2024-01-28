// ----------------------------------------------------------------------
// --------------------  AI: Attack
// -------------------- David Dorrington, UoB Games, 2023
// ---------------------------------------------------------------------

using UnityEngine;

public class DD_Attack : MonoBehaviour
{
    // ---------------------------------------------------------------------
    private DD_GameManager gameManager;
    public float range;
    public float speed = 3;
    public float damage = 30;
    public int teamID = -1;

    // ---------------------------------------------------------------------
    void Start()
    {    // The game manager will be use to access the game board
        gameManager = GameObject.Find("GameManager").GetComponent<DD_GameManager>();
        Destroy(gameObject, (range / speed));    
    }

    // ---------------------------------------------------------------------
    void FixedUpdate()
    {
        MoveAttack();
    }//-----

    private void MoveAttack()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);

    }//----

    /*private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<DD_UnitHealth>())
        {
            collision.gameObject.GetComponent<DD_UnitHealth>().Damage(damage);
        }
        else if (collision.gameObject.GetComponent<MinionHealth>())
        {
            Debug.Log("HIT");
            collision.gameObject.GetComponent<MinionHealth>().Damage(damage);
        }
        Destroy(gameObject);
    }*/


}//==========
