using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpFountian : MonoBehaviour
{
    public float expInteravl;
    private float originExpInteravl;
    public LevelSystem lvls;

    void Start()
    {
        originExpInteravl = expInteravl;
    }

    void Update()
    {
        expInteravl -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (expInteravl < 0)
            {
                expInteravl = originExpInteravl;
                lvls.AddExp(1);
            }
        }
    }
}
