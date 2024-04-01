using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponDisplay : MonoBehaviour
{
    public GameObject weapon;
    private Image weaponImage;

    void Start()
    {
        weapon = transform.Find("Weapon").gameObject;
    }

    public void SetImage(SpriteRenderer img)
    {
        weapon.GetComponent<Image>().sprite = img.sprite;
    }
}
