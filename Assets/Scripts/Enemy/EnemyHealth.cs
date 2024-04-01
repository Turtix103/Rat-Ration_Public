using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public EnemyHealthBarsHandler healthBar;
    public BossBar bossBar;
    public LevelManager manager;


    public float bleedTimer = 0;
    public int bleedDamage = 0;
    public bool bleeding;

    public float burnTimer = 0;
    public int burnDamage = 0;
    public bool burning;

    public float blightTimer = 0;
    public int blightDamage = 0;
    public bool blighted;

    public GameObject damageDisplay;
    public Transform numberParent;
    public Transform damageNumberPoint;

    public GameObject player;

    public int DOTCounter = 1;

    public int expValue;
    public LevelSystem lvls;

    public bool guarding;

    public bool boss;
    public float[] phasePercentages;
    public int currentPhase;

    public bool canGetAttacked;

    public bool dead;

    void Start()
    {
        manager = GameObject.Find("Level Manager").transform.GetComponent<LevelManager>();
        maxHealth = Mathf.RoundToInt(maxHealth * manager.healthMultipliers[manager.levelDifficulty]);

        currentHealth = maxHealth;
        canGetAttacked = true;

        if (!boss)
        {
            healthBar = GameObject.Find("Canvas").GetComponent<EnemyHealthBarsHandler>();
        }
        else
        {
            bossBar = GameObject.Find("Boss Health Bar").GetComponent<BossBar>();
        }

        player = GameObject.Find("Gamer");
        lvls = GameObject.Find("Gamer").GetComponent<LevelSystem>();
        numberParent = GameObject.Find("Numbers").transform;
        StartCoroutine(BugFix());

        InvokeRepeating("DOT",0f, 0.2f);
    }

    void Update()
    {
        bleedTimer -= Time.deltaTime;
        burnTimer -= Time.deltaTime;
        blightTimer -= Time.deltaTime;

        if (bleedTimer < 0 && bleeding)
        {
            bleedDamage = 0;
            gameObject.GetComponent<StatusEffectDisplay>().RemoveEffect(Enum.weaponEffect.bleed);
            bleeding = false;
        }

        if (burnTimer < 0 && burning)
        {
            burnDamage = 0;
            gameObject.GetComponent<StatusEffectDisplay>().RemoveEffect(Enum.weaponEffect.burn);
            burning = false;
        }

        if (blightTimer < 0 && blighted)
        {
            blightDamage = 0;
            gameObject.GetComponent<StatusEffectDisplay>().RemoveEffect(Enum.weaponEffect.blight);
            blighted = false;
        }
    }

    public IEnumerator BugFix()
    {
        if (!boss)
        {
            yield return new WaitUntil(() => GameObject.Find("Canvas").GetComponent<EnemyHealthBarsHandler>().loaded);
            healthBar.SetHealth(currentHealth, maxHealth, gameObject);
        }
        else
        {
            bossBar.SetHealth(maxHealth, currentHealth);
        }
    }


    public void TakeDamge(int damage, Enum.weaponEffect effect, float effectTimer, int effectStrength)
    {
        if (guarding)
        {
            gameObject.GetComponent<EnemyAI>().parried = true;
            guarding = false;
            return;
        }

        if (!canGetAttacked)
            return;

        currentHealth -= damage;

        if (!boss)
            healthBar.SetHealth(currentHealth, maxHealth, gameObject);
        else
            bossBar.SetHealth(maxHealth, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        if (boss)
        {
            if ((float)currentHealth / (float)maxHealth * 100f < phasePercentages[currentPhase])
            {
                Debug.Log(currentHealth + " " + maxHealth + " " + (float)currentHealth / (float)maxHealth * 100f + "%");

                currentPhase++;
                gameObject.GetComponent<KarpAI>().NewPhase();
            }
        }

        ActivateEffect(effect, effectTimer, effectStrength);
        DisplayDamage(damage, player.transform.position.x > transform.position.x ? -3f : 3f, Color.white, 3);
    }

    public void DOT()
    {

        switch (DOTCounter) {
            case 1:
                TakeDOT(bleedDamage, Color.red);
             break;
            case 2:
                TakeDOT(burnDamage, Color.yellow);
             break;
            case 3:
                TakeDOT(blightDamage, Color.green);
             break;
        }

        DOTCounter++;
        if (DOTCounter > 3)
            DOTCounter = 1;
    }

    public void TakeDOT(int damage, Color color)
    {
        currentHealth -= damage;

        if (!boss)
            healthBar.SetHealth(currentHealth, maxHealth, gameObject);
        else
            bossBar.SetHealth(maxHealth, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        if (boss)
        {
            if ((float)currentHealth / (float)maxHealth * 100f < phasePercentages[currentPhase])
            {
                Debug.Log(currentHealth + " " + maxHealth + " " + (float)currentHealth / (float)maxHealth * 100f + "%");

                currentPhase++;
                gameObject.GetComponent<KarpAI>().NewPhase();
            }
        }

        if (damage != 0)
        DisplayDamage(damage, transform.localScale.x == -1 ? -3f : 3f, color, 2);
    }

    private void Die()
    {
        if (dead)
            return;
        if (!boss)
        {
            //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2((FindObjectOfType<PlayerMovement>().gameObject.transform.localScale.x * 5), 5);
            //currentHealth = maxHealth;
            //healthBar.SetHealth(currentHealth, maxHealth, gameObject);
            healthBar.Disable(gameObject);
            lvls.AddExp(expValue);
            currentHealth = maxHealth;
            //Destroy(gameObject);
            gameObject.SetActive(false);
            dead = true;
        }
        else
        {
            bossBar.Deactivate();
            Debug.Log(" FUCK DIE");
            lvls.AddExp(expValue);
            Debug.Log("DIE");
            Destroy(gameObject, 0f);
            dead = true;
        }
    }

    private void DisplayDamage(int damnage, float xPush, Color color, int size)
    {
        GameObject number = Instantiate(damageDisplay, damageNumberPoint.position, damageNumberPoint.rotation);
    //    number.transform.SetParent(numberParent);
     //   number.transform.position = Camera.main.WorldToScreenPoint(number.transform.position);
        number.GetComponent<Rigidbody2D>().AddForce(new Vector2(xPush, 3f), ForceMode2D.Impulse);
        number.GetComponent<TextMeshPro>().text = damnage.ToString();
        number.GetComponent<TextMeshPro>().color = color;
        number.GetComponent<TextMeshPro>().fontSize = size;
        Destroy(number, 0.5f);
    }

    public void ActivateEffect(Enum.weaponEffect effect, float effectTimer, int effectStrength)
    {
        switch (effect)
        {
            case Enum.weaponEffect.none:
                break;
            case Enum.weaponEffect.bleed:
                bleedTimer = effectTimer;
                bleedDamage = effectStrength;
                if (!bleeding)
                    gameObject.GetComponent<StatusEffectDisplay>().AddEffect(effect);
                bleeding = true;
                break;
            case Enum.weaponEffect.burn:
                burnTimer = effectTimer;
                burnDamage = effectStrength;
                if (!burning)
                    gameObject.GetComponent<StatusEffectDisplay>().AddEffect(effect);
                burning = true;
                break;
            case Enum.weaponEffect.blight:
                blightTimer = effectTimer;
                blightDamage = effectStrength;
                if (!blighted)
                    gameObject.GetComponent<StatusEffectDisplay>().AddEffect(effect);
                blighted = true;
                break;
        }
    }
}
