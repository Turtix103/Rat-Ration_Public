using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntWorkerAI : MonoBehaviour
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

    [Header("Combat Mode")]
    public float combatModeTimeLimit;
    public float combatModeTimeLeft;
    public bool combatMode;

    [Header("Speed")]
    public float walkingSpeed;
    public float pursuitSpeed;
    public float chargeSpeed;

    [Header("Slap")]
    public bool slaping;
    public float slapWindUp;
    public float slapPingTimer;
    public float slapDuration;
    public float slapRecovery;
    public Enum.attackType slapAttacktype;
    public EnemyAttackMelee slap;

    [Header("Charge")]
    public float chargeCooldown;
    private float originalChargeCooldown;
    public bool charching;
    public float chargeWindUp;
    public float chargePingTimer;
    public float chargeDuration;
    public float chargeRecovery;
    public Enum.attackType chargeAttacktype;
    public EnemyAttackMelee charge;

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

        originalChargeCooldown = chargeCooldown;
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

        attacking = slaping || charching;

        chargeCooldown -= Time.deltaTime;

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
        else if (playerIsInReach)
        {
            patrol.movementSpeed = 0;
        }

        if (playerIsInRange && !playerIsInProximity && chargeCooldown < 0)
        {
            charching = true;

            StartCoroutine(Charge());
        }
        else if (playerIsInReach && !playerIsInCloseProximity)
        {
            slaping = true;

            StartCoroutine(Slap());
        }
    }

    public IEnumerator Slap()
    {
        patrol.movementSpeed = 0;
        //slap animation gets called
        yield return new WaitForSeconds(slapWindUp);
        ping.Ping(Enum.attackType.yellow);
        yield return new WaitForSeconds(slapPingTimer);
        slap.Attack(Enum.attackType.yellow);
        yield return new WaitForSeconds(slapDuration);
        slap.UnAttack();
        yield return new WaitForSeconds(slapRecovery);
        slaping = false;
    }

    public IEnumerator Charge()
    {
        patrol.movementSpeed = 0;
        //slap animation gets called
        yield return new WaitForSeconds(chargeWindUp);
        ping.Ping(Enum.attackType.yellow);
        yield return new WaitForSeconds(chargePingTimer);
        canPushBack = false;
        charge.Attack(Enum.attackType.yellow);
        patrol.movementSpeed = chargeSpeed;
        yield return new WaitForSeconds(chargeDuration);
        canPushBack = true;
        charge.UnAttack();
        patrol.movementSpeed = 0;
        yield return new WaitForSeconds(chargeRecovery);
        charching = false;
        chargeCooldown = originalChargeCooldown;
    }

    public void RestartCombatTimer()
    {
        combatModeTimeLeft = combatModeTimeLimit;
        combatMode = true;
    }
}
