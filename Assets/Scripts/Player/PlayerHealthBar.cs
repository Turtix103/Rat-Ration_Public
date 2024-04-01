using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    private int maxHealth;
    private int currentHealth;
    private Slider slider;
    private GameObject text;

    public Slider blueSlider;
    void Start()
    {
        maxHealth =  GameObject.Find("Gamer").GetComponent<PlayerHealth>().maxHealth;
        currentHealth = maxHealth;
        slider = gameObject.GetComponent<Slider>();
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
        blueSlider.maxValue = maxHealth / 2;
        blueSlider.value = 0;
        text = GameObject.Find("Player Health Bar Text");
        text.GetComponent<TextMeshProUGUI>().text = $"{currentHealth}/{maxHealth}";
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        if (currentHealth < 0)
            currentHealth = 0;
        slider.value = currentHealth;
        text.GetComponent<TextMeshProUGUI>().text = $"{currentHealth}/{maxHealth}";
        blueSlider.value = currentHealth - maxHealth;
    }

    public void SetHealth(int max, int current)
    {
        currentHealth = current;
        maxHealth = max;

        slider = gameObject.GetComponent<Slider>();
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
        blueSlider.maxValue = maxHealth / 2;
        blueSlider.value = currentHealth - maxHealth;
        text = GameObject.Find("Player Health Bar Text");
        text.GetComponent<TextMeshProUGUI>().text = $"{currentHealth}/{maxHealth}";
    }
}
