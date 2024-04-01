using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform player;
    public bool follow = true;

    // Start is called before the first frame update
    void Start()
    {
        follow = true;
        player = GameObject.Find("Gamer").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (follow)
        transform.position = new Vector3(player.position.x, player.position.y, -10);
    }
}
