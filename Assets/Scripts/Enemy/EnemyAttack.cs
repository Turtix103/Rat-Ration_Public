using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float lifetime;
    public float attackLifetime;
    public int damage;
    public bool canDamage;
    private Collider2D area;
    public Enum.attackType attackType;
    public LevelManager manager;

    void Start()
    {
        area = gameObject.GetComponent<Collider2D>();
        canDamage = true;
        manager = GameObject.Find("Level Manager").transform.GetComponent<LevelManager>();
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        attackLifetime -= Time.deltaTime;

        if (attackLifetime < 0)
            area.enabled = false;

        if (lifetime < 0)
            Destroy(gameObject);
    }

    public void setParameters(int dmg, float lftm, float atcLftm, Enum.attackType atcTp, Transform pos, Transform scale)
    {
        manager = GameObject.Find("Level Manager").transform.GetComponent<LevelManager>();
        Debug.Log("Before");
        damage = Mathf.RoundToInt(dmg * manager.damageMultipliers[manager.levelDifficulty]);
        Debug.Log("after");
        lifetime = lftm;
        attackLifetime = atcLftm;
        attackType = atcTp;
        gameObject.transform.position = pos.position;
        gameObject.transform.localScale = scale.localScale;
        canDamage = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canDamage && collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage, attackType);
            canDamage = false;
        }
    }
}
