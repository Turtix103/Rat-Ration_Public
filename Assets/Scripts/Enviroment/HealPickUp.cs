using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPickUp : MonoBehaviour
{
    public bool notUsed = true;
    public bool triggerStay;
    public PlayerHealth health;
    public int addHeals;

    private void Start()
    {
        health = GameObject.Find("Gamer").GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (triggerStay)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (notUsed)
                {
                    notUsed = false;
                    Use();             
                }
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
    }

    private void Use()
    {
        Debug.Log("HELLO????" + ((health.currentHeals + addHeals) > health.maxHeals));
        if ((health.currentHeals + addHeals) > health.maxHeals)
        {
            notUsed = true;
            Debug.Log("??");
            return;
        }

        health.currentHeals += addHeals;
        health.updateHealth();

        Destroy(gameObject);
    }
}