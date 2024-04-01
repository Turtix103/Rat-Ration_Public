using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTelegraphing : MonoBehaviour
{
    private EnemyAI AI;
    public float telegraphLength;
    public float pingTimer;
    public Sprite normal;
    public Sprite attackWindUp;
    public EnemyAttackPing ping;
    public Enum.attackType currentAttacktype;

    void Start()
    {
        AI = gameObject.GetComponent<EnemyAI>();
        ping = gameObject.GetComponent<EnemyAttackPing>();
    }

    public void InitiateTelegraph(Enum.attackType AT)
    {
        currentAttacktype = AT;
        AI.canTelegraph = false;
        AI.telegraphing = true;
        StartCoroutine(Telegraph());
    }

    public IEnumerator Telegraph()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = attackWindUp;
        yield return new WaitForSeconds(telegraphLength);
        ping.Ping(currentAttacktype);
        AI.pinging = true;
        yield return new WaitForSeconds(pingTimer);
        gameObject.GetComponent<SpriteRenderer>().sprite = normal;
        AI.pinging = false;
        AI.telegraphing = false;
        switch (AI.chosenAttack)
        {
            case EnemyAI.attackType.melee:
                AI.InitiateAttackingMelee(currentAttacktype);
                break;
            case EnemyAI.attackType.meleeCombo:
                AI.AttackingMeleeCombo(currentAttacktype);
                break;
            case EnemyAI.attackType.projectile:
                AI.InitiateAttackingProjectile(currentAttacktype);
                break;
            default:
                break;
        }                                                                                          
    }
}
