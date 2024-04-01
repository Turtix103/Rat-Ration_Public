using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerDetectionLine : MonoBehaviour
{
    [Header("Detection Line")]
    public int lineDistanceFront;
    public int lineDistanceBack;
    public int lineDistanceRange;
    public Transform originPoint;
    private EnemyPatrol patrol;
    public LayerMask player;
    RaycastHit2D playerIsInRange;
    RaycastHit2D playerIsVisible;
    RaycastHit2D playerIsBehind;

    [Header("Attacking")]
    public float attackDelay;
    public bool attacking;
    public bool canAttack;
    private EnemyAttackMelee weapon;
    private EnemyAttackProjectile projectile;
    public enum attackType { melee, projectile, meleeCombo }
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

    void Update()
    {
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
        }
    }

    private void MeleeAI()
    {
        if (playerIsInRange)
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
        if (playerIsInRange)
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
