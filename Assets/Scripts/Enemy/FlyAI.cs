using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAI : MonoBehaviour
{
    //  [Header("AI")]
    public RaycastHit2D playerIsInRange;
    public RaycastHit2D playerIsVisible;
    public RaycastHit2D playerIsBehind;
    public RaycastHit2D playerIsInProximity;
    public RaycastHit2D playerIsInCloseProximity;
    public RaycastHit2D playerIsInReach;
    public EnemyAI AI;
    public EnemyPatrol patrol;
    public EnemyAttackPing ping;
    public bool attacking;
    private bool switched = false;
    public Transform projectilePoint;

    [Header("Combat Mode")]
    public float combatModeTimeLimit;
    public float combatModeTimeLeft;
    public bool combatMode;

    [Header("Speed")]
    public float walkingSpeed;
    public float pursuitSpeed;
    public float retreatSpeed;

    public float chargeSpeed;

    [Header("Spit")]
    public bool spitting;
    public float spitWindUp;
    public float spitPingTimer;
    public float spitDuration;
    public float spitRecovery;
    public Enum.attackType spitAttacktype;
    public EnemyAttackProjectile spit;
    private float spitAngerCoutner;
    public float spitAngerTreshhold;

    [Header("Retreat")]
    public float retreatCooldown;
    private float originalRetreatCooldown;
    public bool retreating;
    public float retreatWindUp;
    public float retreatPingTimer;
    public float retreatAttackDuration;
    public float retreatDuration;
    public float retreatRecovery;
    public Enum.attackType retreatAttacktype;
    public EnemyAttackMelee retreat;
    public GameObject damagingArea;
    public Transform areaPoint;

    [Header("Debug")]
    public bool isBehind;

    public GameObject player;
    public float pushBack = 2f;
    private float pushBackOrigin;
    public bool canPushBack = true;

    private void Start()
    {
        AI = gameObject.GetComponent<EnemyAI>();
        patrol = gameObject.GetComponent<EnemyPatrol>();
        ping = gameObject.GetComponent<EnemyAttackPing>();
        player = GameObject.Find("Gamer");

        originalRetreatCooldown = retreatCooldown;
        pushBackOrigin = pushBack;
    }

    void Update()
    {
        playerIsInRange = AI.playerIsInRange;
        playerIsVisible = AI.playerIsVisible;
        playerIsBehind = AI.playerIsBehind;
        playerIsInProximity = AI.playerIsInProximity;
        playerIsInCloseProximity = AI.playerIsInCloseProximity;
        playerIsInReach = AI.playerIsInReach;

        attacking = spitting || retreating;

        retreatCooldown -= Time.deltaTime;

        patrol.combatMode = combatMode;


        if (playerIsInCloseProximity)
        {
            if (canPushBack)
            {
                // player.GetComponent<PlayerMovement>().TakeKnockback(player.transform.position.x < transform.position.x ? -pushBack : pushBack);
            }
        }

        AIControl();
    }

    private void AIControl()
    {
        isBehind = false;

        if (attacking)
            return;


        combatModeTimeLeft -= Time.deltaTime;

        if (combatModeTimeLeft <= 0)
            combatMode = false;

        if (!combatMode)
            combatModeTimeLeft = 0;


        if (playerIsVisible)
        {
            RestartCombatTimer();
            patrol.movementSpeed = pursuitSpeed;
        }
        else if (playerIsBehind && !switched)
        {
            RestartCombatTimer();
            patrol.switchDirection();
            switched = true;
        }
        else if (combatMode)
        {
            patrol.movementSpeed = ((Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x)) > -0.15 && (Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x) < 0.15)) ? 0 : pursuitSpeed;

            if ((Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x) > 0.15 && patrol.facing == EnemyPatrol.direction.right) || (Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x) < -0.15 && patrol.facing == EnemyPatrol.direction.left))
                patrol.switchDirection();
        }
        else
            patrol.movementSpeed = walkingSpeed;

        if (!playerIsBehind)
            switched = false;

        if (playerIsInCloseProximity && playerIsVisible)
        {
            patrol.movementSpeed = -2;
            //patrol.movementSpeed = 0;
        }

        if (playerIsInReach)
        {
            patrol.movementSpeed = -pursuitSpeed;
        }
        else if (!playerIsInReach && playerIsInRange)
        {
            patrol.movementSpeed = 0;
        }

        if (playerIsInRange)
        {
            if (spitAngerCoutner < 0)
                spitAngerCoutner = 0;
            spitAngerCoutner += Time.deltaTime;
        }
        else
        {
            spitAngerCoutner -= Time.deltaTime;
        }

        if ((playerIsInRange && !playerIsInReach) || spitAngerCoutner > spitAngerTreshhold)
        {
            spitting = true;
            spitAngerCoutner = 0;

            StartCoroutine(Spit());
        }
        else if (playerIsInProximity && retreatCooldown < 0)
        {
            retreating = true;

            StartCoroutine(Retreat());
        }

        if (!patrol.groundedBack && playerIsInReach)
        {
            patrol.movementSpeed = 0;
        }
    }

    public IEnumerator Spit()
    {
        patrol.movementSpeed = 0;
        //slap animation gets called
        yield return new WaitForSeconds(spitWindUp);
        ping.Ping(Enum.attackType.yellow);
        yield return new WaitForSeconds(spitPingTimer);
        spit.Shoot(patrol.facing, projectilePoint, Enum.attackType.yellow);
        yield return new WaitForSeconds(spitRecovery);
        spitting = false;
    }

    public IEnumerator Retreat()
    {
        patrol.movementSpeed = 0;
        //slap animation gets called
        yield return new WaitForSeconds(retreatWindUp);
        ping.Ping(Enum.attackType.green);
        yield return new WaitForSeconds(retreatPingTimer);
        canPushBack = false;
        retreat.Attack(Enum.attackType.green);
        GameObject area = Instantiate(damagingArea, areaPoint);
        area.transform.position = new Vector2(areaPoint.position.x, areaPoint.position.y + 0.23f);
        area.transform.parent = null;
        yield return new WaitForSeconds(retreatAttackDuration);
        retreat.UnAttack();
        patrol.movementSpeed = retreatSpeed;
        yield return new WaitForSeconds(retreatDuration);
        canPushBack = true;
        patrol.movementSpeed = 0;
        yield return new WaitForSeconds(retreatRecovery);
        retreating = false;
        retreatCooldown = originalRetreatCooldown;
    }

    public void RestartCombatTimer()
    {
        combatModeTimeLeft = combatModeTimeLimit;
        combatMode = true;
    }
}
