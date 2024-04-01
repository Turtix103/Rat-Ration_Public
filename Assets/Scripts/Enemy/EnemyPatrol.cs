using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public enum direction { left, right }

    public Transform groundCheck;
    public Transform groundBackCheck;
    public Transform wallCheck;
    public LayerMask ground;
    public bool grounded;
    public bool groundedBack;
    public bool nearWall;
    public direction facing;
    Rigidbody2D rb;
    public float checkerRadius;
    public float movementSpeed;
    public bool waiting;
    public bool turning = false;
    public bool combatMode;

    public Vector2 constantVelocity;
    public Vector2 knockbackVelocity;
    private Vector2 finalVelocity;

    public EnemyAI AI;

    public bool canTurn;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        AI = GetComponent<EnemyAI>();
        facing = direction.left;
        waiting = false;
        canTurn = true;
    }

    void Update()
    {
        EnviromentCheck();
        Walk();

    //    StartCoroutine(Velocity());
    }

    private IEnumerator Velocity()
    {
        yield return new WaitForEndOfFrame();

        finalVelocity = constantVelocity + knockbackVelocity;
        rb.velocity = new Vector2 (finalVelocity.x , rb.velocity.y);
    }

    public void Walk()
    {
        if (grounded && !nearWall)
        {
            switch (facing)
            {
                case direction.left:
                    rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
                    //rb.AddForce(new Vector2(-movementSpeed, 0), ForceMode2D.Force);
                    break;

                case direction.right:
                    rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
                    //rb.AddForce(new Vector2(movementSpeed, 0), ForceMode2D.Force);
                    break;
            }
        }
        else if ((!grounded || nearWall) && rb.velocity.x != 0)

        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else if (!waiting && !AI.combatMode)
        {
            if (combatMode)
                return;

            StartCoroutine(switchDirectionSlowly());
            waiting = true;
        }
    }

    private void EnviromentCheck()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, checkerRadius, ground);
        groundedBack = Physics2D.OverlapCircle(groundBackCheck.position, checkerRadius, ground);
        nearWall = Physics2D.OverlapCircle(wallCheck.position, checkerRadius, ground);
    }

    private IEnumerator switchDirectionSlowly()
    {
        if (!canTurn)
            yield break;

        rb.velocity = new Vector2(0, rb.velocity.y);
        yield return new WaitForSeconds(2f);
        switch (facing)
        {
            case direction.left:
                facing = direction.right;
                waiting = !waiting;
                gameObject.transform.localScale = new Vector2(-1, gameObject.transform.localScale.y);
                transform.Find("Status Display").transform.localScale = new Vector2(-1, gameObject.transform.localScale.y);
                break;

            case direction.right:
                facing = direction.left;
                waiting = !waiting;
                gameObject.transform.localScale = new Vector2(1, gameObject.transform.localScale.y);
                transform.Find("Status Display").transform.localScale = new Vector2(1, gameObject.transform.localScale.y);
                break;
        }
    }

    public void TakeKnockback(float knockback)
    {
        //rb.velocity += new Vector2(knockback * (gameObject.transform.localScale.x == -1 ? -1f : 1f), 0);
        rb.AddForce(new Vector2(knockback * (gameObject.transform.localScale.x == -1 ? -1f : 1f), 0), ForceMode2D.Impulse);
    }

    public void switchDirection()
    {
        if (turning)
            return;

        turning = true;

        if (AI.attacking || AI.pinging)
            return;

        rb.velocity = new Vector2(0, rb.velocity.y);
        switch (facing)
        {
            case direction.left:
                facing = direction.right;
                gameObject.transform.localScale = new Vector2(-1, gameObject.transform.localScale.y);
                transform.Find("Status Display").transform.localScale = new Vector2(-1, gameObject.transform.localScale.y);
                turning = false;
                break;

            case direction.right:
                facing = direction.left;
                gameObject.transform.localScale = new Vector2(1, gameObject.transform.localScale.y);
                transform.Find("Status Display").transform.localScale = new Vector2(1, gameObject.transform.localScale.y);
                turning = false;
                break;
        }
    }
}
