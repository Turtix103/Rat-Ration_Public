using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public float speed = 0;
    public int damage = 0;
    private Rigidbody2D rb;
    public Enum.attackType currentAttackType;
    public GameObject destroyParticle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage, currentAttackType);
            Destroy(gameObject);
        }
        if (collision.tag == "Ground")
        {
            Instantiate(destroyParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
