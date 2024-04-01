using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDecay : MonoBehaviour
{
    public float decaytime;

    void Awake()
    {
        StartCoroutine(Decay());
    }

    private IEnumerator Decay()
    {
        yield return new WaitForSeconds(decaytime);
        Destroy(gameObject);
    }
}
