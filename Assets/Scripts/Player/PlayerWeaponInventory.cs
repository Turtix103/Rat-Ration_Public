using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponInventory : MonoBehaviour
{

    public List<GameObject> weaponsNear;
    public bool pickingUpWeapon = false;
    public Image sprite;
    private Transform parent;
    private PlayerAttack weaponHandler;

    public GameObject gameUI;
    public GameObject chooseUI;

    public Image gameSecondarySlot;
    public Image gamePrimarySlot;
    public Image chooseSecondarySlot;
    public Image choosePrimarySlot;

    public GameObject tempWeapon;

    void Start()
    {
        weaponHandler = FindObjectOfType<PlayerAttack>();
        parent = GameObject.Find("Weapons").transform;
    }

    void Update()
    {
        if (weaponsNear.Count > 0)
        {
            int closestWeapon = ClosestWeapon();
            foreach (GameObject weapon in weaponsNear)
            {
                weapon.transform.Find("Item").GetComponent<WeaponItemDisplay>().displayText = false;
            }
            weaponsNear[closestWeapon].transform.Find("Item").GetComponent<WeaponItemDisplay>().displayText = true;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            swapWeapons();
        }
    }

    public void PickUpWeapon(GameObject weapon)
    {
        StartCoroutine(PickUpWeaponCoroutine(weapon));
    }

    public IEnumerator PickUpWeaponCoroutine(GameObject weapon)
    {
        StopTime.Stop();

        choosePrimarySlot.sprite = gamePrimarySlot.sprite;
        chooseSecondarySlot.sprite = gameSecondarySlot.sprite;
        gameUI.SetActive(false);
        chooseUI.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D));

        gameUI.SetActive(true);
        chooseUI.SetActive(false);

        StopTime.Resume();

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (weaponHandler.weaponP != null)
                ThrowOutWeapon(weaponHandler.weaponP.transform);
            weaponsNear.Remove(weapon);
            weaponHandler.SetWeaponP(weapon);
            GameObject.Find("Canvas/Game UI/Weapon Display Primary").GetComponent<UIWeaponDisplay>().SetImage(weapon.transform.Find("Item").GetComponent<SpriteRenderer>());
            StartCoroutine(PickUpCooldown());
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (weaponHandler.weaponS != null)
                ThrowOutWeapon(weaponHandler.weaponS.transform);
            weaponsNear.Remove(weapon);
            weaponHandler.SetWeaponS(weapon);
            GameObject.Find("Canvas/Game UI/Weapon Display Secondary").GetComponent<UIWeaponDisplay>().SetImage(weapon.transform.Find("Item").GetComponent<SpriteRenderer>());
            StartCoroutine(PickUpCooldown());
        }
    }

    public IEnumerator PickUpCooldown()
    {
        yield return new WaitForEndOfFrame();
        pickingUpWeapon = false;
    }

    public void PickUpNearestWeapon()
    {
        if (pickingUpWeapon)
            return;

        pickingUpWeapon = true;

        int closestWeapon = ClosestWeapon();

        weaponsNear[closestWeapon].transform.Find("Item").gameObject.SetActive(false);
        weaponsNear[closestWeapon].transform.Find("Item").GetComponent<WeaponItemDisplay>().isInInv = true;
        PickUpWeapon(weaponsNear[closestWeapon]);
    }

    public int ClosestWeapon()
    {
        float shortestDistance = Vector3.Distance(transform.position, weaponsNear[0].transform.position);
        int closestWeapon = 0;
        int index = 0;
        foreach (GameObject weapon in weaponsNear)
        {
            if (shortestDistance > Vector3.Distance(transform.position, weapon.transform.position))
            {
                shortestDistance = Vector3.Distance(transform.position, weapon.transform.position);
                closestWeapon = index;
            }
            index++;
        }
        return closestWeapon;
    }

    public void ThrowOutWeapon(Transform weapon)
    {
        weapon.localPosition = new Vector2(1 * gameObject.transform.localScale.x, 0);
        weapon.SetParent(parent);
        weapon.Find("Item").gameObject.SetActive(true);
        weaponsNear.Add(weapon.gameObject);
        weapon.Find("Item").GetComponent<WeaponItemDisplay>().isInInv = false;
        weapon.Find("Item").transform.localPosition = new Vector2(0, 0);
        weapon.GetComponent<WeaponCombo>().pickedUp = false;
        weapon.localScale = new Vector2(1, 1);
    }

    public void swapWeapons()
    {
        if (weaponHandler.weaponP != null)
        {
            tempWeapon = weaponHandler.weaponP.gameObject;

            if (weaponHandler.weaponS != null)
                weaponHandler.SetWeaponP(weaponHandler.weaponS.gameObject);
            else
                weaponHandler.UnsetWeaponP();

            weaponHandler.SetWeaponS(tempWeapon);


            gamePrimarySlot.sprite = gameSecondarySlot.sprite;
            gameSecondarySlot.sprite = tempWeapon.transform.Find("Item").GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            tempWeapon = weaponHandler.weaponS.gameObject;

            if (weaponHandler.weaponP != null)
                weaponHandler.SetWeaponS(weaponHandler.weaponS.gameObject);
            else
                weaponHandler.UnsetWeaponS();

            weaponHandler.SetWeaponP(tempWeapon);

            gameSecondarySlot.sprite = gamePrimarySlot.sprite;
            gamePrimarySlot.sprite = tempWeapon.transform.Find("Item").GetComponent<SpriteRenderer>().sprite;
        }


    }
}
