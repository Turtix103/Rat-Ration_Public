using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTime : MonoBehaviour
{
    private static bool stoped;

    void Start()
    {
        stoped = false;
    }

    public static void Stop()
    {
        if (!stoped)
        {
            Time.timeScale = 0f;
            stoped = true;
        }
    }

    public static void Resume()
    {
        if (stoped)
        {
            Time.timeScale = 1f;
            stoped = false;
        }
    }
}
