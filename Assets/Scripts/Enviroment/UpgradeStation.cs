using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStation : MonoBehaviour
{
    public GameObject player;

    public bool triggerStay;

    public WeaponUpgradeMenu menu;

    void Start()
    {
        player = GameObject.Find("Gamer");
        menu = GameObject.Find("Canvas").GetComponent<WeaponUpgradeMenu>();
    }

    private void Update()
    {
        if (triggerStay) {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(UpgradeCoroutine());
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
    //    if (collision.gameObject.CompareTag("Player"))
    //        GameObject.Find("Canvas").GetComponent<TextDisplayHandler>().DisplayText(gameObject);

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

    public IEnumerator UpgradeCoroutine()
    {
        StopTime.Stop();
        menu.upgradeMenu.SetActive(true);
        menu.UpdateValues();
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D));
        menu.upgradeMenu.SetActive(false);
        StopTime.Resume();

        if (Input.GetKeyDown(KeyCode.A))
        {
            player.transform.Find("Primary Weapon").GetComponentInChildren<WeaponLevel>().UpgradeWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            player.transform.Find("Secondary Weapon").GetComponentInChildren<WeaponLevel>().UpgradeWeapon(1);
        }
        Destroy(gameObject);
    }
}
