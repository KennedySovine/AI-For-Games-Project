using UnityEngine;

public class Turret_Attack : MonoBehaviour
{
    public float range = 50;
    public float speed = 3;
    public float damage = 30;
    public string teamTarget;
    public bool lockedOn = false;
    public GameObject bullet;

    // ---------------------------------------------------------------------
    void Start()
    {   
    }

    // ---------------------------------------------------------------------
    void FixedUpdate()
    {
        Vector3 currentPostion = transform.position;
        Vector3 enemyPosition = new Vector3(0, 0, 0);
        GameObject attackItem;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(teamTarget);
        foreach (GameObject enemy in enemies)
        {
            if (!lockedOn)
            {
                enemyPosition = enemy.transform.position;
                //Debug.Log(Vector3.Distance(transform.position, enemyPosition));
                lockedOn = true;
            }
            if (Vector3.Distance(currentPostion, enemyPosition) <= range && lockedOn)
            {
                //Debug.Log(gameObject + " locked on");
                //sendAttack(enemyPosition);
                if (enemy.GetComponent<DD_UnitHealth>())
                {
                    enemy.GetComponent<DD_UnitHealth>().Damage(damage);
                    Destroy(bullet);
                }
            }
            if (Vector3.Distance(currentPostion, enemyPosition) > range)
            {
                lockedOn = false;
            }
        }
    }

    /*private void sendAttack(Vector3 enemyPos)
    {
        if (!bullet) return;
        transform.LookAt(enemyPos);
        GameObject turretAttack = Instantiate(bullet, gameObject.transform);
        turretAttack.GetComponent<DD_Attack>().range = range;
        turretAttack.transform.SetParent(null);
        turretAttack.transform.position = Vector3.MoveTowards(turretAttack.transform.position, enemyPos, 1);
    }*/
}
