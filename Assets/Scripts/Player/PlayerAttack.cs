using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject weaponP;
    public GameObject weaponS;
    public WeaponCombo comboP;
    public WeaponCombo comboS;
    public GameObject PrimaryInv;
    public GameObject SecondaryInv;

    public bool attackingP;
    public bool attackingS;
    public bool attacking;
    public bool windingUp;

    // Start is called before the first frame update
    void Start()
    {
        SetWeaponP(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponP != null && weaponS != null)
        {
            attackingP = !comboP.canAttack;
            attackingS = !comboS.canAttack;
            attacking = attackingP || attackingS;
            windingUp = comboP.windingUp || comboS.windingUp;
        }
        else if (weaponP != null)
        {
            attacking = !comboP.canAttack;
            windingUp = comboP.windingUp;
        }
        else if (weaponS != null)
        {
            attacking = !comboS.canAttack;
            windingUp = comboS.windingUp;
        }

        if (!InputHandler.inputAllowed)
            return;

        if (Input.GetMouseButtonDown(0) && !attackingS)
        {
            //attack.gameObject.SetActive(true);
            comboP.AttackRequest();
            StartCoroutine(Wait());
        }
        else if (Input.GetMouseButtonDown(1) && !attackingP)
        {
            //attack.gameObject.SetActive(true);
            comboS.AttackRequest();
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        //  attack.gameObject.SetActive(false);
    }

    public void SetWeaponP(GameObject weapon)
    {
        weaponP = weapon;
        if (weaponP == null)
            return;
        weapon.transform.SetParent(PrimaryInv.transform);
        weaponP.transform.localPosition = new Vector2(0, 0);
        weaponP.transform.localScale = new Vector2(1, weaponP.transform.localScale.y);
        weaponP.transform.GetComponent<WeaponCombo>().pickedUp = true;
        comboP = weaponP.GetComponent<WeaponCombo>();
    }
    public void SetWeaponS(GameObject weapon)
    {
        weaponS = weapon;
        if (weaponS == null)
            return;
        weapon.transform.SetParent(SecondaryInv.transform);
        weaponS.transform.localPosition = new Vector2(0, 0);
        weaponS.transform.localScale = new Vector2(1, weaponS.transform.localScale.y);
        weaponS.transform.GetComponent<WeaponCombo>().pickedUp = true;
        comboS = weaponS.GetComponent<WeaponCombo>();
    }

    public void UnsetWeaponP()
    {
        weaponP = null;
        comboP = null;
    }

    public void UnsetWeaponS()
    {
        weaponS = null;
        comboS = null;
    }
}