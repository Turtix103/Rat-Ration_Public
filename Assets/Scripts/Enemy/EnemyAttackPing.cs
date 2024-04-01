using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPing : MonoBehaviour
{
    public Transform pingPoint;
    public ParticleSystem yellowPing;
    public ParticleSystem bluePing;
    public ParticleSystem greenPing;
    public ParticleSystem redPing;

    void Start()
    {
        pingPoint = transform.Find("Ping Point");
    }
    public void Ping(Enum.attackType type)
    {
        switch (type)
        {
            case Enum.attackType.yellow:
                Instantiate(yellowPing, pingPoint.position, yellowPing.transform.rotation);
                break;
            case Enum.attackType.blue:
                Instantiate(bluePing, pingPoint.position, bluePing.transform.rotation);
                break;
            case Enum.attackType.green:
                Instantiate(greenPing, pingPoint.position, greenPing.transform.rotation);
                break;
            case Enum.attackType.red:
                Instantiate(redPing, pingPoint.position, redPing.transform.rotation);
                break;
            default:
                break;
        }
    }
}
