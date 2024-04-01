using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackDirectional : MonoBehaviour
{
    public float speed = 0;
    public int damage = 0;
    public Enum.attackType currentAttackType;
    public Transform direction;
    public LevelManager manager;
    public GameObject destroyParticle;

    private void Start()
    {
        manager = GameObject.Find("Level Manager").transform.GetComponent<LevelManager>();
    }
    void Update()
    {
        //rb.velocity = new Vector2(speed, rb.velocity.y);

        Vector3 newPosition = Vector3.Lerp(transform.position, direction.position, Time.deltaTime * speed);

        transform.position = newPosition;
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

    public void SetParameters(float spd, int dmg)
    {
        speed = spd;
        StartCoroutine(stupidFuckingBug(dmg));
    }

    public IEnumerator stupidFuckingBug(int dmg)
    {
        yield return new WaitForEndOfFrame();
        damage = Mathf.RoundToInt(dmg * manager.damageMultipliers[manager.levelDifficulty]);
    }
}
