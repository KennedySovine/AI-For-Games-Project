using UnityEngine;

public class Turret_Attack : MonoBehaviour
{
    public float range = 50;
    public float speed = 3;
    public float damage = 30;
    public float coolDown = 0.833F;
    public float nextAttackTime = 0;
    public string teamTarget;
    public string enemyMinion;
    public bool lockedOn = false;
    public GameObject bullet;

    // ---------------------------------------------------------------------
    void Start()
    {
        range = 10;
        coolDown = 0.833F;
    }

    // ---------------------------------------------------------------------
    void FixedUpdate()
    {
        Vector3 currentPostion = transform.position;
        Vector3 enemyPosition = new Vector3(0, 0, 0);
        GameObject attackItem;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(teamTarget);
        GameObject[] enemyMinions = GameObject.FindGameObjectsWithTag(enemyMinion);
        //Check Minions First
        foreach (GameObject enemy in enemyMinions)
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
                sendAttack(enemyPosition);
                enemy.GetComponent<MinionHealth>().Damage(damage);
            }
            if (Vector3.Distance(currentPostion, enemyPosition) > range)
            {
                lockedOn = false;
            }
            break;
        }

        //Check enemy units
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
                sendAttack(enemyPosition);
                enemy.GetComponent<DD_UnitHealth>().Damage(damage);
            }
            if (Vector3.Distance(currentPostion, enemyPosition) > range)
            {
                lockedOn = false;
            }
        }
    }

    private void sendAttack(Vector3 enemyPos)
    {
        if (!bullet) return;
        if (nextAttackTime < Time.time)
        {
            GameObject turretAttack = Instantiate(bullet, gameObject.transform);
            turretAttack.transform.LookAt(enemyPos);
            turretAttack.GetComponent<DD_Attack>().range = range;
            turretAttack.transform.SetParent(null);
            turretAttack.transform.position = Vector3.MoveTowards(turretAttack.transform.position, enemyPos, 1);
            nextAttackTime = Time.time + coolDown;
        }
    }
}
