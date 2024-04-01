using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpMenu : MonoBehaviour
{
    public PlayerHealth health;
    public GameObject menu;

    public int additionalHeals;
    public int healthPercentage;
    public int damagePercentage;
    public int effecicencyPercentage;

    public int allowedEfficencyUpgrades;

    public void AddHeal()
    {
        health.currentHeals += additionalHeals;
        menu.SetActive(false);
        health.updateHealth();
        StopTime.Resume();
    }

    public void AddHealth()
    {
        health.currentMaxHealthBoost += healthPercentage;
        menu.SetActive(false);
        StopTime.Resume();
    }

    public void AddDamage()
    {
        health.DamageBoost += damagePercentage;
        menu.SetActive(false);
        StopTime.Resume();
    }

    public void AddEfficency()
    {
        if (allowedEfficencyUpgrades <= 0)
            return;

        health.healEfficency += effecicencyPercentage;
        health.overHeal += effecicencyPercentage;
        allowedEfficencyUpgrades--;
        menu.SetActive(false);
        StopTime.Resume();
    }
}
