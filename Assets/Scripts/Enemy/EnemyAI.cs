using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("AI")]
    public Enum.playerDetection detectionType;
    private EnemyPatrol patrol;
    public LayerMask player;
    public RaycastHit2D playerIsInRange;
    public RaycastHit2D playerIsVisible;
    public RaycastHit2D playerIsBehind;
    public RaycastHit2D playerIsInProximity;
    public RaycastHit2D playerIsInCloseProximity;
    public RaycastHit2D playerIsInReach;
    public bool parried;

    [Header("Detection Line")]
    public int lineDistanceFront;
    public int lineDistanceBack;
    public int lineDistanceRange;
    public Transform originPoint;

    [Header("Detection Box")]
    public Transform boxCastPosition;
    public Vector2 boxCastSizeFront;
    public Vector2 boxCastSizeBack;
    public Vector2 boxCastSizeRange;
    public Vector2 boxCastSizeProximity;
    public Vector2 boxCastSizeCloseProximity;
    public Vector2 boxCastSizeReach;
    private Vector3 facingDirection;
    private Vector3 calculatedOffsetFront;
    private Vector3 calculatedOffsetBack;
    private Vector3 calculatedOffsetRange;
    private Vector3 calculatedOffsetProximity;
    private Vector3 calculatedOffsetCloseProximity;
    private Vector3 calculatedOffsetReach;

    [Header("Attacking")]
    public float attackDelay;
    public bool attacking;
    public bool canAttack;
    private EnemyAttackMelee weapon;
    private EnemyAttackProjectile projectile;
    public enum attackType { melee, projectile, meleeCombo, custom }
    public attackType chosenAttack;
    public Transform projectilePoint;
    public bool comboAttacking;
    public Enum.attackType currentAttacktype;

    [Header("Combat Mode")]
    public float combatModeTimeLimit;
    public float combatModeTimeLeft;
    public bool combatMode;

    [Header("Telegraphing")]
    private EnemyAttackTelegraphing telegraph;
    public bool canTelegraph;
    public bool telegraphing;
    public bool pinging;

    void Start()
    {
        patrol = gameObject.GetComponent<EnemyPatrol>();
        telegraph = gameObject.GetComponent<EnemyAttackTelegraphing>();
        weapon = gameObject.GetComponent<EnemyAttackMelee>();
        projectile = gameObject.GetComponent<EnemyAttackProjectile>();
        projectilePoint = transform.Find("Projectile Point");
        attacking = false;
        canAttack = true;
        combatMode = false;
        combatModeTimeLeft = 0f;
        canTelegraph = true;
        telegraphing = false;
        comboAttacking = false;
    }

    void FixedUpdate()
    {
        switch (detectionType)
        {
            case Enum.playerDetection.line:
                playerIsInRange = Physics2D.Linecast(originPoint.position, new Vector2(originPoint.position.x - (lineDistanceRange * gameObject.transform.localScale.x), originPoint.position.y), player);

                playerIsVisible = Physics2D.Linecast(originPoint.position, new Vector2(originPoint.position.x - (lineDistanceFront * gameObject.transform.localScale.x), originPoint.position.y), player);

                playerIsBehind = Physics2D.Linecast(originPoint.position, new Vector2(originPoint.position.x + (lineDistanceBack * gameObject.transform.localScale.x), originPoint.position.y), player);


                Debug.DrawLine(originPoint.position, new Vector2(originPoint.position.x - (lineDistanceFront * gameObject.transform.localScale.x), originPoint.position.y), Color.yellow);
                Debug.DrawLine(originPoint.position, new Vector2(originPoint.position.x - (lineDistanceRange * gameObject.transform.localScale.x), originPoint.position.y), Color.red);
                Debug.DrawLine(originPoint.position, new Vector2(originPoint.position.x + (lineDistanceBack * gameObject.transform.localScale.x), originPoint.position.y), Color.yellow);


                if (playerIsBehind && !playerIsVisible && !attacking)
                    patrol.switchDirection();

                combatModeTimeLeft -= Time.deltaTime;

                if (combatModeTimeLeft <= 0)
                    combatMode = false;

                if (!combatMode)
                    combatModeTimeLeft = 0;
                break;

            case Enum.playerDetection.box:
                playerIsVisible = Physics2D.BoxCast(boxCastPosition.position + calculatedOffsetFront, boxCastSizeFront, 0f, facingDirection, 0f, player);

                playerIsBehind = Physics2D.BoxCast(boxCastPosition.position + calculatedOffsetBack, boxCastSizeBack, 0f, facingDirection, 0f, player);

                playerIsInRange = Physics2D.BoxCast(boxCastPosition.position + calculatedOffsetRange, boxCastSizeRange, 0f, facingDirection, 0f, player);

                playerIsInProximity = Physics2D.BoxCast(boxCastPosition.position + calculatedOffsetProximity, boxCastSizeProximity, 0f, facingDirection, 0f, player);

                playerIsInCloseProximity = Physics2D.BoxCast(boxCastPosition.position + calculatedOffsetCloseProximity, boxCastSizeCloseProximity, 0f, facingDirection, 0f, player);

                playerIsInReach = Physics2D.BoxCast(boxCastPosition.position + calculatedOffsetReach, boxCastSizeReach, 0f, facingDirection, 0f, player);

                facingDirection = transform.localScale.x == -1 ? Vector3.right : Vector3.left;

                calculatedOffsetFront = new Vector3(boxCastSizeFront.x / 2 * (transform.localScale.x == -1 ? 1 : -1), boxCastSizeFront.y / 2 - 0.687f, 0);
                calculatedOffsetBack = new Vector3(boxCastSizeBack.x / 2 * (transform.localScale.x == 1 ? 1 : -1), boxCastSizeBack.y / 2 - 0.687f, 0);
                calculatedOffsetRange = new Vector3(boxCastSizeRange.x / 2 * (transform.localScale.x == -1 ? 1 : -1), boxCastSizeRange.y / 2 - 0.687f, 0);
                calculatedOffsetProximity = new Vector3(0, boxCastSizeProximity.y / 2 - 0.687f, 0);
                calculatedOffsetCloseProximity = new Vector3(0, boxCastSizeCloseProximity.y / 2 - 0.687f, 0);
                calculatedOffsetReach = new Vector3(boxCastSizeReach.x / 2 * (transform.localScale.x == -1 ? 1 : -1), boxCastSizeReach.y / 2 - 0.687f, 0);

                break;

            case Enum.playerDetection.radius:
                break;
        }

        switch (chosenAttack)
        {
            case attackType.melee:
                MeleeAI();
                break;
            case attackType.meleeCombo:
                MeleeComboAI();
                break;
            case attackType.projectile:
                ProjectileAI();
                break;
            case attackType.custom:
                
                break;
        }
    }

    void OnDrawGizmos()
    {
        facingDirection = transform.localScale.x == -1 ? Vector3.right : Vector3.left;

        calculatedOffsetFront = new Vector3(boxCastSizeFront.x / 2 * (transform.localScale.x == -1 ? 1 : -1), boxCastSizeFront.y / 2 - 0.687f, 0);
        calculatedOffsetBack = new Vector3(boxCastSizeBack.x / 2 * (transform.localScale.x == 1 ? 1 : -1), boxCastSizeBack.y / 2 - 0.687f, 0);
        calculatedOffsetRange = new Vector3(boxCastSizeRange.x / 2 * (transform.localScale.x == -1 ? 1 : -1), boxCastSizeRange.y / 2 - 0.687f, 0);
        calculatedOffsetProximity = new Vector3(0, boxCastSizeProximity.y / 2 - 0.687f, 0);
        calculatedOffsetCloseProximity = new Vector3(0, boxCastSizeCloseProximity.y / 2 - 0.687f, 0);
        calculatedOffsetReach = new Vector3(boxCastSizeReach.x / 2 * (transform.localScale.x == -1 ? 1 : -1), boxCastSizeReach.y / 2 - 0.687f, 0);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCastPosition.position + calculatedOffsetFront, new Vector2(boxCastSizeFront.x, boxCastSizeFront.y));

        Gizmos.DrawWireCube(boxCastPosition.position + calculatedOffsetBack, new Vector2(boxCastSizeBack.x, boxCastSizeBack.y));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxCastPosition.position + calculatedOffsetRange, new Vector2(boxCastSizeRange.x, boxCastSizeRange.y));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCastPosition.position + calculatedOffsetProximity, new Vector2(boxCastSizeProximity.x, boxCastSizeProximity.y));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCastPosition.position + calculatedOffsetReach, new Vector2(boxCastSizeReach.x, boxCastSizeReach.y));

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(boxCastPosition.position + calculatedOffsetCloseProximity, new Vector2(boxCastSizeCloseProximity.x, boxCastSizeCloseProximity.y));

    }

    private void MeleeAI()
    {
        if (playerIsInReach)
        {
            patrol.movementSpeed = 0;
            RestartCombatTimer();
            if (canTelegraph)
                telegraph.InitiateTelegraph(currentAttacktype);
        }
        else if (attacking || telegraphing)
        {
            patrol.movementSpeed = 0;
        }
        else if (playerIsVisible)
        {
            patrol.movementSpeed = 3;
            RestartCombatTimer();
        }
        else if (combatMode)
        {
            patrol.movementSpeed = ((Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x)) > -0.15 && (Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x) < 0.15)) ? 0 : 3;

            if ((Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x) > 0.15 && patrol.facing == EnemyPatrol.direction.right) || (Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x) < -0.15 && patrol.facing == EnemyPatrol.direction.left))
                patrol.switchDirection();
        }
        else
            patrol.movementSpeed = 1;
    }

    private void MeleeComboAI()
    {
        if (playerIsInReach)
        {
            patrol.movementSpeed = 0;
            RestartCombatTimer();
            if (canTelegraph && !comboAttacking)
                telegraph.InitiateTelegraph(currentAttacktype);
        }
        else if (attacking || telegraphing)
        {
            patrol.movementSpeed = 0;
        }
        else if (playerIsVisible)
        {
            patrol.movementSpeed = 3;
            RestartCombatTimer();
        }
        else if (combatMode)
        {
            patrol.movementSpeed = ((Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x)) > -0.15 && (Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x) < 0.15)) ? 0 : 3;

            if ((Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x) > 0.15 && patrol.facing == EnemyPatrol.direction.right) || (Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x) < -0.15 && patrol.facing == EnemyPatrol.direction.left))
                patrol.switchDirection();
        }
        else
            patrol.movementSpeed = 1;
    }

    private void ProjectileAI()
    {
        if (playerIsVisible)
        {
            patrol.movementSpeed = 0;
            RestartCombatTimer();
            if (canTelegraph)
                telegraph.InitiateTelegraph(currentAttacktype);
        }
        else if (attacking || telegraphing)
        {
            patrol.movementSpeed = 0;
        }
        else if (combatMode)
        {
            patrol.movementSpeed = ((Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x)) > -0.15 && (Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x) < 0.15)) ? 0 : 3;

            if ((Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x) > 0.15 && patrol.facing == EnemyPatrol.direction.right) || (Mathf.Abs(transform.position.x) - Mathf.Abs(GameObject.Find("Gamer").transform.position.x) < -0.15 && patrol.facing == EnemyPatrol.direction.left))
                patrol.switchDirection();
        }
        else
            patrol.movementSpeed = 1;
    }

    public void InitiateAttackingMelee(Enum.attackType AT)
    {
        StartCoroutine(AttackingMelee(AT));
    }

    private IEnumerator AttackingMelee(Enum.attackType AT)
    {
        canAttack = false;
        weapon.Attack(AT);
        weapon.InitiateRecovery();
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
        canTelegraph = true;
    }

    public void AttackingMeleeCombo(Enum.attackType AT)
    {
        comboAttacking = true;
        weapon.AttackCombo(AT);
    }

    public void InitiateAttackingProjectile(Enum.attackType AT)
    {
        StartCoroutine(AttackingProjectile(AT));
    }

    private IEnumerator AttackingProjectile(Enum.attackType AT)
    {
        canAttack = false;
        projectile.Shoot(patrol.facing, projectilePoint, AT);
        projectile.InitiateRecovery();
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
        canTelegraph = true;
    }


    public void RestartCombatTimer()
    {
        combatModeTimeLeft = combatModeTimeLimit;
        combatMode = true;
    }
}
