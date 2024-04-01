using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanArea : MonoBehaviour
{
    public float lifetime;
    public float attackLifetime;
    public int damage;
    public bool canDamage;
    private Collider2D area;
    public Enum.attackType attackType;

    public float warmUp;
    private EnemyAttackPing ping;
    public float pingTimer;
    public GameObject warningArea;
    public bool activated;
    public GameObject attackSprite;
    public LevelManager manager;

    void Start()
    {
        area = gameObject.GetComponent<Collider2D>();
        ping = gameObject.GetComponent<EnemyAttackPing>();
        manager = GameObject.Find("Level Manager").transform.GetComponent<LevelManager>();
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        attackLifetime -= Time.deltaTime;
        warmUp -= Time.deltaTime;

        if (lifetime < 0 && activated) 
        {
            area.enabled = false;
            canDamage = false;
            activated = false;
            attackSprite.SetActive(false);
        }

        if (warmUp < 0 && !canDamage && activated)
        {
            canDamage = true;
            area.enabled = true;
            warningArea.SetActive(false);
            attackSprite.SetActive(true);
        }
    }

    public void setParameters(int dmg, float lftm, Enum.attackType atcTp, float wrmP, float pngTmr)
    {
        damage = Mathf.RoundToInt(dmg * manager.damageMultipliers[manager.levelDifficulty]);
        lifetime = lftm + wrmP;
        attackType = atcTp;
        warmUp = wrmP;
        pingTimer = pngTmr;
        warningArea.SetActive(true);
        activated = true;

        StartCoroutine(Ping());
    }

    public IEnumerator Ping()
    {
        yield return new WaitForSeconds(warmUp - pingTimer);
        ping.Ping(attackType);
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
