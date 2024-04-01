using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUpgradeMenu : MonoBehaviour
{
    public GameObject upgradeMenu;

    public Image weaponP;
    public Image weaponS;
    public int pLevel;
    public int slevel;
    public TextMeshProUGUI primaryText;
    public TextMeshProUGUI secondaryText;

    public PlayerWeaponInventory playerInv;
    public PlayerAttack weapons;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateValues()
    {
        if (weapons.weaponP != null)
        {
            weaponP.sprite = playerInv.gamePrimarySlot.sprite;
            primaryText.text = $"Current level: {weapons.weaponP.GetComponent<WeaponLevel>().currentWeaponLevel}";
        }

        if (weapons.weaponS != null)
        {
            weaponS.sprite = playerInv.gameSecondarySlot.sprite;
            secondaryText.text = $"Current level: {weapons.weaponS.GetComponent<WeaponLevel>().currentWeaponLevel}";
        }
    }
}
