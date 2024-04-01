using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemDisplay : MonoBehaviour
{
    public float distanceToText;
    public GameObject player;
    public Transform text;
    public Transform description;
    private PlayerWeaponInventory inv;
    public float timeToDescription;
    public float timeToDescriptionOrigin;
    public bool playerNear = false;
    public bool displayDescription = false;
    public bool isInInv = false;
    public bool displayText ;

    void Start()
    {
        player = GameObject.Find("Gamer");
        text = transform.Find("Text");
        description = transform.Find("Description");
        text.gameObject.SetActive(false);
        description.gameObject.SetActive(false);
        inv = FindObjectOfType<PlayerWeaponInventory>();
        timeToDescriptionOrigin = timeToDescription;
    }

    void Update()
    {

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (playerNear)
        {
            timeToDescription -= Time.deltaTime;
            if (timeToDescription <= 0)
                displayDescription = true;
        }
        else
        {
            timeToDescription = timeToDescriptionOrigin;
            displayDescription = false;
        }

        if (distance <= distanceToText)
        {
            if (!playerNear)
                inv.weaponsNear.Add(transform.parent.gameObject);
            playerNear = true;
            //text.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (inv.weaponsNear.Count > 1)
                {
                    inv.PickUpNearestWeapon();
                }
                else
                {
                    inv.PickUpWeapon(transform.parent.gameObject);
                    gameObject.SetActive(false);
                }
            }
            if (displayDescription)
                description.gameObject.SetActive(true);
            else
                description.gameObject.SetActive(false);
        }
        else
        {
            if (playerNear)
                inv.weaponsNear.Remove(transform.parent.gameObject);

            //text.gameObject.SetActive(false);
            playerNear = false;
        }

        if (displayText)
            text.gameObject.SetActive(true);
        if (!displayText || !playerNear)
            text.gameObject.SetActive(false);

    }
}
