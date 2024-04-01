using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossBar : MonoBehaviour
{
    private int maxHealth;
    private int currentHealth;
    public Slider slider;

    public GameObject border;
    public GameObject fill;
    void Start()
    {
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        if (currentHealth < 0)
            currentHealth = 0;
        slider.value = currentHealth;
    }

    public void SetHealth(int max, int current)
    {
        currentHealth = current;
        maxHealth = max;

        slider.maxValue = maxHealth;
        slider.value = currentHealth;
    }

    public void Activate()
    {
        border.gameObject.SetActive(true);
        fill.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        border.gameObject.SetActive(false);
        fill.gameObject.SetActive(false);
    }
}
