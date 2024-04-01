using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponAttack : MonoBehaviour
{
    Collider2D attackArea;
    public float damage;
    public float knockBack;
    public WeaponLevel level;

    void Start()
    {
        attackArea = gameObject.GetComponent<Collider2D>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            //  collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 5);

            float addidition = GameObject.Find("Gamer").GetComponent<PlayerHealth>().currentDamageBoost / 100 * (int)Math.Round((level.levelMultipliers[level.currentWeaponLevel - 1] * damage));

            Debug.Log(GameObject.Find("Gamer").GetComponent<PlayerHealth>().currentDamageBoost);
            Debug.Log(damage);
            Debug.Log(addidition);
            Debug.Log(60 / 100 * 10);


            collision.gameObject.GetComponent<EnemyHealth>().TakeDamge((int)Math.Round((level.levelMultipliers[level.currentWeaponLevel - 1] * damage) + addidition), gameObject.GetComponentInParent<WeaponCombo>().effect, gameObject.GetComponentInParent<WeaponCombo>().effectTimer, gameObject.GetComponentInParent<WeaponCombo>().effectStrength);

            collision.gameObject.GetComponent<EnemyPatrol>().TakeKnockback(knockBack);

        }
    }
}
