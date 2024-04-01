using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float projectileSpeed;
    public int damage;
    public float recoveryTime;
    private EnemyAI AI;
    public Enum.attackType currentAttacktype;
    public LevelManager manager;

    private void Start()
    {
        AI = transform.GetComponent<EnemyAI>();
        manager = GameObject.Find("Level Manager").transform.GetComponent<LevelManager>();
        damage = Mathf.RoundToInt(damage * manager.damageMultipliers[manager.levelDifficulty]);
    }
    public void Shoot(EnemyPatrol.direction direction, Transform originPoint, Enum.attackType AT)
    {
        GameObject GO = Instantiate(projectile, originPoint.position, originPoint.rotation);
        GO.GetComponent<ProjectileAttack>().currentAttackType = AT;
        switch (direction) {
            case EnemyPatrol.direction.left:
                GO.GetComponent<ProjectileAttack>().speed = -projectileSpeed;
                break;
            case EnemyPatrol.direction.right:
                GO.GetComponent<ProjectileAttack>().speed = projectileSpeed;
                GO.GetComponent<Transform>().localScale = new Vector2(-1, gameObject.transform.localScale.y);
                break;
        }
        GO.GetComponent<ProjectileAttack>().damage = damage;
    }
    public void InitiateRecovery()
    {
        StartCoroutine(Recover());
    }

    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(recoveryTime);
        AI.attacking = false;
    }
}
