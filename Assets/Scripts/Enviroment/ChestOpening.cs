using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChestOpening : MonoBehaviour
{
    public GameObject[] weapons;

    System.Random gen = new System.Random();

    public bool chestClosed = true;
    public bool triggerStay;

    public Sprite openChest;
    public GameObject chestLid;
    public GameObject chestLock;
    public Vector3 offsetLid;
    public Vector3 offsetLock;

    private void Update()
    {
        if (triggerStay)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (chestClosed)
                    ChestOpen();

                chestClosed = false;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            triggerStay = true;
            gameObject.GetComponent<TextDisplay>().DisplayText();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            triggerStay = false;
            gameObject.GetComponent<TextDisplay>().UnDisplayText();
        }

        //    GameObject.Find("Canvas").GetComponent<TextDisplayHandler>().UnDisplayText(gameObject);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        //    if (collision.gameObject.CompareTag("Player"))
        //        GameObject.Find("Canvas").GetComponent<TextDisplayHandler>().DisplayText(gameObject);
    }

    private void ChestOpen()
    {
        Instantiate(weapons[gen.Next(0, weapons.Length)], transform.position, transform.rotation).GetComponent<WeaponLevel>().currentWeaponLevel = GameObject.Find("Level Manager").GetComponent<LevelManager>().levelDifficulty + 1;

        gameObject.GetComponent<SpriteRenderer>().sprite = openChest;
        Instantiate(chestLid, transform.position + offsetLid, transform.rotation);
        Instantiate(chestLock, transform.position + offsetLock, transform.rotation);
        gameObject.GetComponent<TextDisplay>().UnDisplayText();
    }
}
