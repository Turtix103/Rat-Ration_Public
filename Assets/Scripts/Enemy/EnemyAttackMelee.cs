using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackMelee : MonoBehaviour
{
    public Collider2D attackArea;
    public int damage;
    public EnemyAI AI;
    public float recoveryTime;
    public Transform attacksParent;
    private List<Transform> attacks = new List<Transform>();
    public int currentAttack = 0;
    private GameObject Player;
    private EnemyAttackTelegraphing telegraph;
    public float attackCombiDistance;
    public Enum.attackType currentAttacktype;
    public bool parried;
    public LevelManager manager;

    void Start()
    {
        foreach (Transform child in attacksParent)
        {
            attacks.Add(child);
            child.gameObject.SetActive(false);
        }

        Player = GameObject.Find("Gamer");
        attackArea.gameObject.SetActive(false);

        if (AI == null)
        AI = GetComponent<EnemyAI>();

        telegraph = GetComponent<EnemyAttackTelegraphing>();

        manager = GameObject.Find("Level Manager").transform.GetComponent<LevelManager>();
        damage = Mathf.RoundToInt(damage * manager.damageMultipliers[manager.levelDifficulty]);
    }

    public void Attack(Enum.attackType AT)
    {
        if (!AI.attacking)
        {
            parried = false;
            currentAttacktype = AT;
            attackArea.gameObject.SetActive(true);
            AI.attacking = true;
        }
    }

    public void UnAttack()
    {
        attackArea.gameObject.SetActive(false);
        AI.attacking = false;
    }

    public void AttackCombo(Enum.attackType AT)
    {
        if (!AI.attacking)
        {
            currentAttacktype = AT;
            attacks[currentAttack].gameObject.SetActive(true);
            AI.attacking = true;
            InitiateRecovery();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            parried = collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage, currentAttacktype);
        }
    }

    public void InitiateRecovery()
    {
        StartCoroutine(Recover());
    }

    private IEnumerator Recover()
    {
        attackArea.gameObject.SetActive(false);
        yield return new WaitForSeconds(recoveryTime);
        switch (AI.chosenAttack)
        {
            case EnemyAI.attackType.melee:
                attackArea.gameObject.SetActive(false);
                break;
            case EnemyAI.attackType.meleeCombo:
                attacks[currentAttack].gameObject.SetActive(false);
                AI.canAttack = true;
                AI.canTelegraph = true;
                break;
        }
        AI.attacking = false;

        if (currentAttack >= attacks.Count || !AI.comboAttacking || !((Vector3.Distance(transform.position, Player.transform.position) < attackCombiDistance)))
        {
            currentAttack = 0;
            AI.comboAttacking = false;
        }
        else if (AI.comboAttacking && ((Vector3.Distance(transform.position, Player.transform.position) < attackCombiDistance)))
        {
            currentAttack++;

            if (currentAttack >= attacks.Count)
            {
                currentAttack = 0;
                AI.comboAttacking = false;
            }
            else
            telegraph.InitiateTelegraph(currentAttacktype);
        }
    }
}
