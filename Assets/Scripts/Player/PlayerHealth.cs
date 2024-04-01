using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    private GameObject canvas;
    public GameObject death;
    private PlayerMovement movement;

    public int maxHeals;
    public int currentHeals;
    public float healEfficency;
    public float overHeal;

    public int DamageBoost;
    public float currentDamageBoost;
    public int currentMaxHealthBoost;

    public Image hurtEffect;
    public float hurtSeverity;
    public float hurtDecrease;
    public int hurtIncrease;
    public int maxHurt;
    public int lowHealthHurt;

    public GameObject healDisplay;
    void Start()
    {
        currentHealth = maxHealth;
        currentHeals = maxHeals;
        canvas = GameObject.Find("Player Health Bar");
        death = GameObject.Find("Canvas");
        movement = gameObject.GetComponent<PlayerMovement>();
    }

    public void Update()
    {
        Color tempColor = hurtEffect.color;
        tempColor.a = hurtSeverity / 255f;
        hurtEffect.color = tempColor;

        hurtSeverity -= Time.deltaTime * hurtDecrease;

        if (hurtSeverity < 0)
            hurtSeverity = 0;

        if ((float)currentHealth / (float)maxHealth < 0.3f)
            hurtSeverity = lowHealthHurt;

        if (!InputHandler.inputAllowed)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentHeals > 0 && currentHealth != maxHealth)
            {
                Heal();
            }
        }

        /*
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentHeals = maxHeals;
            updateHealth();
        } */
    }

    public bool TakeDamage(int damage, Enum.attackType attackType)
    {
        if (movement.parrying && attackType != Enum.attackType.green && attackType != Enum.attackType.red)
        {
            movement.parryInterupt = true;
            return true;
        }

        if (movement.rolling && attackType != Enum.attackType.blue && attackType != Enum.attackType.red)
            return false;

        if (movement.blocking && attackType != Enum.attackType.green && attackType != Enum.attackType.red)
            damage /= 2;

        Debug.Log(damage);

        hurtSeverity += hurtIncrease;
        if (hurtSeverity > maxHurt)
            hurtSeverity = maxHurt;


        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;

        canvas.GetComponent<PlayerHealthBar>().TakeDamage(damage);

        if (currentHealth <= 0)
            death.GetComponent<PlayerDeath>().KillPlayer();

        return false;
    }

    public void ResetHealth()
    {
        canvas.GetComponent<PlayerHealthBar>().TakeDamage(-maxHealth);
        currentHealth = maxHealth;

    }

    public void Heal()
    {
        maxHealth += (maxHealth / 100 * currentMaxHealthBoost);
        currentDamageBoost += DamageBoost;
        int antidamage = Mathf.RoundToInt(maxHealth / 100 * healEfficency);
        int healthRemainder = maxHealth - currentHealth;
        currentHealth += antidamage;
        if (currentHealth > maxHealth)
          currentHealth = maxHealth;

        antidamage -= healthRemainder;

        float allowedOverHeal = (float)maxHealth / 100f * (float)overHeal;
        if (antidamage > allowedOverHeal)
            antidamage = (int)allowedOverHeal;

        currentHealth += antidamage;

        currentHeals--;

        updateHealth();
        canvas.GetComponent<PlayerHealthBar>().SetHealth(maxHealth, currentHealth);
    }

    public void updateHealth()
    {
        healDisplay.GetComponent<TextMeshProUGUI>().text = $"Heals: {currentHeals}/3";
    }
}
