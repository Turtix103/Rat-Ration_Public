using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderExit : MonoBehaviour
{
    private PlayerMovement move;

    void Start()
    {
        move = GameObject.Find("Gamer").transform.GetComponent<PlayerMovement>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (move.onLadder)
            move.GetOffLadder();
        }
    }
}
