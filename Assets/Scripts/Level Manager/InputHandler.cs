using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static bool inputAllowed = true;
    public static bool ResetVelocity = false;

    public static void AllowInput()
    {
        inputAllowed = true;
        ResetVelocity = false;
    }

    public static void ForbidInput()
    {
        inputAllowed = false;
        ResetVelocity = true;
    }
} 
