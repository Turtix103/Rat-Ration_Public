using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonamiCode : MonoBehaviour
{
    public bool stage1, stage2, stage3, stage4, stage5, stage6, stage7, stage8, stage9, stage10;
    public float requestTimer;
    public float originRequestTimer;
    public GameObject secretRoom;
    // up up down down left right left right roll jump
    void Start()
    {
        originRequestTimer = requestTimer;
    }

    void Update()
    {
        requestTimer -= Time.deltaTime;

        if (requestTimer < 0)
        {
            stage1 = false;
            stage2 = false;
            stage3 = false;
            stage4 = false;
            stage5 = false;
            stage6 = false;
            stage7 = false;
            stage8 = false;
            stage9 = false;
            stage10 = false;
        }

        if (Input.GetKeyDown(KeyCode.W) && !stage1 && !stage2 && !stage3 && !stage4 && !stage5 && !stage6 && !stage7 && !stage8 && !stage9 && !stage10)
        {
            stage1 = true;
            requestTimer = originRequestTimer;
        }
        else if (Input.GetKeyDown(KeyCode.W) && stage1 && !stage2 && !stage3 && !stage4 && !stage5 && !stage6 && !stage7 && !stage8 && !stage9 && !stage10)
        {
            stage2 = true;
            requestTimer = originRequestTimer;
        }
        if (Input.GetKeyDown(KeyCode.S) && stage1 && stage2 && !stage3 && !stage4 && !stage5 && !stage6 && !stage7 && !stage8 && !stage9 && !stage10)
        {
            stage3 = true;
            requestTimer = originRequestTimer;
        }
        else if (Input.GetKeyDown(KeyCode.S) && stage1 && stage2 && stage3 && !stage4 && !stage5 && !stage6 && !stage7 && !stage8 && !stage9 && !stage10)
        {
            stage4 = true;
            requestTimer = originRequestTimer;
        }
        if (Input.GetKeyDown(KeyCode.A) && stage1 && stage2 && stage3 && stage4 && !stage5 && !stage6 && !stage7 && !stage8 && !stage9 && !stage10)
        {
            stage5 = true;
            requestTimer = originRequestTimer;
        }
        if (Input.GetKeyDown(KeyCode.D) && stage1 && stage2 && stage3 && stage4 && stage5 && !stage6 && !stage7 && !stage8 && !stage9 && !stage10)
        {
            stage6 = true;
            requestTimer = originRequestTimer;
        }
        if (Input.GetKeyDown(KeyCode.A) && stage1 && stage2 && stage3 && stage4 && stage5 && stage6 && !stage7 && !stage8 && !stage9 && !stage10)
        {
            stage7 = true;
            requestTimer = originRequestTimer;
        }
        if (Input.GetKeyDown(KeyCode.D) && stage1 && stage2 && stage3 && stage4 && stage5 && stage6 && stage7 && !stage8 && !stage9 && !stage10)
        {
            stage8 = true;
            requestTimer = originRequestTimer;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && stage1 && stage2 && stage3 && stage4 && stage5 && stage6 && stage7 && stage8 && !stage9 && !stage10)
        {
            stage9 = true;
            requestTimer = originRequestTimer;
        }
        if (Input.GetKeyDown(KeyCode.Space) && stage1 && stage2 && stage3 && stage4 && stage5 && stage6 && stage7 && stage8 && stage9 && !stage10)
        {
            stage10 = true;
            requestTimer = originRequestTimer;
        }
        if (stage1 && stage2 && stage3 && stage4 && stage5 && stage6 && stage7 && stage8 && stage9 && stage10)
        {
            secretRoom.SetActive(true);
            stage1 = false;
            stage2 = false;
            stage3 = false;
            stage4 = false;
            stage5 = false;
            stage6 = false;
            stage7 = false;
            stage8 = false;
            stage9 = false;
            stage10 = false;
        }
    }
}
