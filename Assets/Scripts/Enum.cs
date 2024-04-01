using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enum : MonoBehaviour
{
    public enum attackType { yellow, blue, green, red };
    public enum playerDetection { line, box, radius };
    public enum weaponEffect { none, bleed, burn, blight, stun };
}
