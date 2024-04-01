using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleAI : MonoBehaviour
{
    [Header("AI")]
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
    public EnemyHealth health;

    [Header("Combat Mode")]
    public float combatModeTimeLimit;
    public float combatModeTimeLeft;
    public bool combatMode;

    [Header("Speed")]
    public float walkingSpeed;
    public float pursuitSpeed;
    public float chargeSpeed;

    [Header("Uppercut")]
    public bool uppercutting;
    public float uppercutWindUp;
    public float uppercutPingTimer;
    public float uppercutDuration;
    public float uppercutRecovery;
    public Enum.attackType uppercutAttacktype;
    public EnemyAttackMelee uppercut;

    [Header("Smack")]
    public bool smacking;
    public float smackWindUp;
    public float smackPingTimer;
    public float smackDuration;
    public float smackRecovery;
    public Enum.attackType smackAttacktype;
    public EnemyAttackMelee smack;

    [Header("Hiding")]
    public GameObject particle1;
    public GameObject particle2;
    public GameObject sprite;
    public float hideCooldown;
    private float originHideCooldown;
    public bool hidden;

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
        health = gameObject.GetComponent<EnemyHealth>();
        player = GameObject.Find("Gamer");

        originHideCooldown = hideCooldown;
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

        attacking = uppercutting || smacking;

        hideCooldown -= Time.deltaTime;

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

        if (hidden)
        {
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

            if (playerIsInReach && !playerIsInCloseProximity)
            {
                uppercutting = true;

                StartCoroutine(Uppercut());
            }
        }
        else
        {
            if (playerIsBehind && !switched)
            {
                patrol.switchDirection();
                switched = true;
            }

            if (!playerIsBehind)
                switched = false;

            if (playerIsInReach && !playerIsInProximity)
            {
                smacking = true;

                StartCoroutine(Smack());
            }
            else if(!playerIsInReach && hideCooldown < 0)
            {
                Hide();
            }
        }

    }

    public IEnumerator Uppercut()
    {
        patrol.movementSpeed = 0;
        yield return new WaitForSeconds(uppercutWindUp);
        ping.Ping(Enum.attackType.green);
        yield return new WaitForSeconds(uppercutPingTimer);
        uppercut.Attack(Enum.attackType.green);
        Appear();
        yield return new WaitForSeconds(uppercutDuration);
        uppercut.UnAttack();
        yield return new WaitForSeconds(uppercutRecovery);
        uppercutting = false;
    }

    public IEnumerator Smack()
    {
        yield return new WaitForSeconds(smackWindUp);
        ping.Ping(Enum.attackType.yellow);
        yield return new WaitForSeconds(smackPingTimer);
        smack.Attack(Enum.attackType.yellow);
        yield return new WaitForSeconds(smackDuration);
        smack.UnAttack();
        yield return new WaitForSeconds(smackRecovery);
        Hide();
        smacking = false;
    }

    public void Hide()
    {
        sprite.SetActive(false);
        particle1.SetActive(true);
        particle2.SetActive(true);
        hidden = true;
        health.canGetAttacked = false;
    }

    public void Appear()
    {
        sprite.SetActive(true);
        particle1.SetActive(false);
        particle2.SetActive(false);
        hidden = false;
        health.canGetAttacked = true;
        hideCooldown = originHideCooldown;
    }

    public void RestartCombatTimer()
    {
        combatModeTimeLeft = combatModeTimeLimit;
        combatMode = true;
    }
}
