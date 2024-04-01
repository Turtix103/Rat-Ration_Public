using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLevel : MonoBehaviour
{
    public int currentWeaponLevel;

    public float[] levelMultipliers = new float[] {1f, 1.5f, 2.25f, 3.375f, 5f, 7f, 11f, 17f, 25f, 35f, 50f, 75f, 125f};

    public void UpgradeWeapon(int upgrade)
    {
        currentWeaponLevel += upgrade;
    }
}
