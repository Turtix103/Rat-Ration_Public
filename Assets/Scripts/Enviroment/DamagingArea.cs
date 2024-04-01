using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingArea : MonoBehaviour
{
    public float repeating;
    public int damage;
    public float duration;
    public bool playerIn;
    public PlayerHealth player;
    public LevelManager manager;

    void Start()
    {
        InvokeRepeating("Damage", 0f, repeating);
        player = GameObject.Find("Gamer").GetComponent<PlayerHealth>();
        manager = GameObject.Find("Level Manager").transform.GetComponent<LevelManager>();
        damage = Mathf.RoundToInt(damage * manager.damageMultipliers[manager.levelDifficulty]);
    }

    void Update()
    {
        duration -= Time.deltaTime;

        if (duration <= 0)
            Destroy(gameObject);
    }

    public void Damage()
    {
        if (playerIn)
        {
            player.TakeDamage(damage, Enum.attackType.green);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerIn = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerIn = false;
        }
    }
}
