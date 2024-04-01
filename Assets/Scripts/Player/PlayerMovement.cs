using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ping;
    public Transform pingPoint;

    [Header("Movement")]
    public float movementSpeed;
    public float originSpeed;
    public float stopingSpeed;
    public float knockbackForce;
    public float knockbackDecayRate;
    public bool decayKnockback;
    public bool moving;
    public bool beingKnocked;

    [Header("Jumping")]
    public Transform[] groundCheckers;
    public float checkerRadius;
    public LayerMask ground;
    public bool grounded;
    public float jumpForce;
    public int bonusJumps;
    public int bonusJumpsLeft;

    [Header("Falling")]
    public float fallMultiplier = 2.5f;
    public float terminalVelocity;

    [Header("Climbing")]
    public bool canClimb;
    public float climbSpeed = 3f;
    public float climbDistance = 1f;
    public LayerMask climbableLayer;
    private bool isClimbing = false;
    public Vector3 climbTarget;

    [Header("Scaling")]
    public float scalableHeight;
    public float scalingDistance = 0.5f;
    public Transform scaleCheck;
    public Transform scaleHelp;
    public Transform scaleDebug;

    [Header("Ladder")]
    public float ladderMovementSpeed;
    public LayerMask ladderLayer;
    public float ladderDistance;
    [SerializeField] Tilemap ladderMap;
    public bool onLadder = false;
    private Vector3 ladderMapOffset;

    [Header("Crouching")]
    public float crouchingSpeed;
    public bool crouched = false;
    public float crouchJumpForce;

    [Header("Rolling")]
    public bool rolling = false;
    public float rollForce;
    public float rollTime;
    private float rollTimeTemp;
    public bool interuptRoll = false;
    public float rollCooldown;
    public float rollCooldownOrigin;
    public bool canRoll;
    public float rollRequestTimer;
    public float originRollRequestTimer;

    [Header("Blocking")]
    public bool blocking = false;
    public bool parrying;
    public float parryingTimer;
    private float originParryingTimer;
    public float parryDuration;
    private float originParryDuration;
    public GameObject parrySprite;
    public bool parryInterupt;
    public int parryRepeat;
    public bool parryStop;
    public float startParryTimer;
    private float originStartParryTimer;
    public bool parried;

    [Header("Misc")]
    public Animator animator;
    Rigidbody2D rb;
    public bool funnyRotations = false;
    public bool attacking;
    public bool windingUp;
    public PlayerAttack attack;

    [Header("Map")]
    public GameObject map;
    public bool isMapOn;

    public void onSceneLoaded()
    {
        gameObject.transform.position = new Vector2(0, 0);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scaleCheck = GameObject.Find("Wall Scale Check").transform;
        scaleHelp = GameObject.Find("Wall Scale Helper").transform;
        scaleDebug = GameObject.Find("Wall Scale Debug").transform;
        originSpeed = movementSpeed;
        originParryDuration = parryDuration;
        originParryingTimer = parryingTimer;
        rollCooldownOrigin = rollCooldown;
        canRoll = true;
        attack = gameObject.GetComponent<PlayerAttack>();
        originRollRequestTimer = rollRequestTimer;
        originStartParryTimer = startParryTimer;
        rollRequestTimer = 0;
        isMapOn = true;
    }

    void Update()
    {
        parryDuration -= Time.deltaTime;
        parryingTimer -= Time.deltaTime;
        rollRequestTimer -= Time.deltaTime;
        //startParryTimer -= Time.deltaTime;
        windingUp = attack.windingUp;
        attacking = attack.attacking;
        canRoll = !attacking || (attacking && windingUp);

        if (Input.GetKeyDown(KeyCode.P))
            Instantiate(ping, pingPoint.position, pingPoint.rotation);

        if (InputHandler.ResetVelocity)
        {
            rb.velocity = new Vector2(0, 0);
            interuptRoll = true;
            Roll();
        }

        if (!InputHandler.inputAllowed)
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
            ToggleMap();

        if (Input.GetKeyDown(KeyCode.LeftShift) && !rolling)
            rollRequestTimer = originRollRequestTimer;

        rollCooldown -= Time.deltaTime;

        if (knockbackForce < 0.2f && knockbackForce > -0.2f)
        {
            /* knockbackForce = 0;
             decayKnockback = false; */
            beingKnocked = false;

        }

        if (!onLadder)
        Blocking();

        if (!attacking || (attacking && windingUp))
        Movement();

        TerminalVelocity();

      /*  if (decayKnockback)
        KnockbackDecay(); */

        if (!blocking && !parrying)
        {
            if (!windingUp || !attacking)
            Jump();

            if (rollCooldown < 0 && canRoll)
            Rolling();
        }

        if (!onLadder && !crouched && !rolling && !blocking)
        {
            GroundCheck();

            MovementDecay();

            if (canClimb)
            Climbing();

            if (!windingUp || !attacking)
            Scaling();

            //Phasing();

            if (!windingUp || !attacking)
            LadderCheck();

            Crouching();
        }
        else if (onLadder && !blocking)
        {
            LadderMovement();
        } 
        else if (!blocking)
        {

            GroundCheck();

            if (!attacking)
            CrouchMovement();

            Crouching();
        }

        if (Input.GetKey(KeyCode.Z))
        {
            if (!funnyRotations)
            {
                rb.constraints &= ~RigidbodyConstraints2D.FreezeRotation;
                funnyRotations = true;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                funnyRotations = false;
            }
        }
    }

    public void ToggleMap()
    {
        if (isMapOn)
        {
            map.SetActive(false);
            isMapOn = false;
        }
        else
        {
            map.SetActive(true);
            isMapOn = true;
        }
    }

    private void Rolling()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift) || rollRequestTimer > 0) && !rolling)
        {
            rolling = true;
            rollRequestTimer = 0;
            rollTimeTemp = rollTime;
            animator.SetBool("Rolling", true);
            GetOffLadder();
        }
        if (rolling)
        {
            Roll();
            CkeckForInterupt();
        }
    }

    private void Roll()
    {
        if (rollTimeTemp <= 0 || interuptRoll)
        {
            rolling = false;
           // Debug.Log("");
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetBool("Rolling", false);
            interuptRoll = false;
            rollCooldown = rollCooldownOrigin;
            return;
        }

        rollTimeTemp -= Time.deltaTime;

        switch (gameObject.transform.localScale.x)
        {
            case 1:
                rb.velocity = new Vector2(rollForce, rb.velocity.y);
                break;
            case -1:
                rb.velocity = new Vector2(-rollForce, rb.velocity.y);
                break;
        }
    }

    private void CkeckForInterupt()
    {
        if (Input.GetKeyDown(KeyCode.A))
            InterupRoll();
        else if (Input.GetKeyDown(KeyCode.D))
            InterupRoll();
        else if (Input.GetKeyDown(KeyCode.S))
            InterupRoll();
        else if (Input.GetKeyDown(KeyCode.Space))
            InterupRoll();
        else if (attacking && !windingUp)
            InterupRoll();

    }

    private void InterupRoll()
    {
        if (rolling)
        {
            interuptRoll = true;
            Roll(); 
        }
    }



    private void Movement()
    {
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
            gameObject.transform.localScale = new Vector2(1, gameObject.transform.localScale.y);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
            gameObject.transform.localScale = new Vector2(-1, gameObject.transform.localScale.y);
        }
        else if (rb.velocity.x != 0)
        {
            if (rb.velocity.x < 0.2 && rb.velocity.x > -0.2)
                rb.velocity = new Vector2(0, rb.velocity.y);
            else
                rb.velocity = new Vector2(rb.velocity.x - (Time.deltaTime * 7 * (Mathf.Abs(rb.velocity.x) / rb.velocity.x)), rb.velocity.y);
        }
    }

    private void MovementV2()
    {
        Vector2 movement = new Vector2(0,0);
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            movement = new Vector2(0, rb.velocity.y);
            moving = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement = new Vector2(movementSpeed, rb.velocity.y);
            gameObject.transform.localScale = new Vector2(1, gameObject.transform.localScale.y);
            moving = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movement = new Vector2(-movementSpeed, rb.velocity.y);
            gameObject.transform.localScale = new Vector2(-1, gameObject.transform.localScale.y);
            moving = true;
        }
        else if (rb.velocity.x != 0)
        {
            moving = false;
            if (rb.velocity.x < 0.2 && rb.velocity.x > -0.2)
                movement = new Vector2(0, rb.velocity.y);
            else
                movement = new Vector2(rb.velocity.x - (Time.deltaTime * 7 * (Mathf.Abs(rb.velocity.x) / rb.velocity.x)), rb.velocity.y);
        }

        //if attacking movement should be 0

        /*  if (!moving &&!decayKnockback)
              rb.velocity = new Vector2(0, rb.velocity.y);
          else
              rb.velocity = movement + new Vector2(knockbackForce, 0); */

        if (!beingKnocked)
            rb.velocity = movement;
    }

    private void LadderMovement()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
            rb.velocity = new Vector2(rb.velocity.x, 0);
        else if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector2(rb.velocity.x, ladderMovementSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector2(rb.velocity.x, -ladderMovementSpeed);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((grounded || bonusJumpsLeft > 0 ) || onLadder)
            {

                if (!grounded && !onLadder)
                {
                    rb.velocity = Vector2.up * (jumpForce - 1);
                    bonusJumpsLeft -= 1;
                }
                else
                {
                    rb.velocity = Vector2.up * jumpForce;
                }

                if (!crouched)
                    GetOffLadder();
            }
        }
    }

    private void TerminalVelocity()
    {
        if (rb.velocity.y < 5)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

        if (rb.velocity.y < -terminalVelocity)
            rb.velocity = new Vector2(rb.velocity.x, -terminalVelocity);
    }

    private void MovementDecay()
    {
        if (Input.GetKeyUp(KeyCode.A))
            rb.velocity = new Vector2(-stopingSpeed, rb.velocity.y);
        if (Input.GetKeyUp(KeyCode.D))
            rb.velocity = new Vector2(stopingSpeed, rb.velocity.y);

        if (rb.velocity.x != 0)
        {
            if (rb.velocity.x < 0.2 && rb.velocity.x > -0.2)
                rb.velocity = new Vector2(0, rb.velocity.y);
            else
                rb.velocity = new Vector2(rb.velocity.x - (Time.deltaTime * 7 * (Mathf.Abs(rb.velocity.x) / rb.velocity.x)), rb.velocity.y);
        }
    }

    private void Climbing()
    {
        if (IsNearClimbableSurface() && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !isClimbing)
        {
            rb.AddForce(Vector2.up * climbSpeed);
            Vector2 velocity = rb.velocity;
            velocity.y = Mathf.Clamp(Math.Abs(velocity.x), 0, 4);
            rb.velocity = velocity;
        }
    }

    private bool IsNearClimbableSurface()
    {
        return Physics2D.Linecast(transform.position, transform.position + ((gameObject.transform.lossyScale.x == -1) ? Vector3.left : Vector3.right) * climbDistance, climbableLayer);
    }

    private void Scaling()
    {
        RaycastHit2D scan = IsNearScalableSurface();
        RaycastHit2D able = CanScaleSurface();
        RaycastHit2D inGround = IsInTheGround();

        if (scan && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !inGround)
        {
            RaycastHit2D scanDown = Physics2D.Raycast(new Vector2(scan.point.x + ((gameObject.transform.lossyScale.x == -1) ? -0.1f : 0.1f), scan.point.y + scalableHeight), Vector2.down, scalableHeight, ground);

            float distanceToUp = scalableHeight - scanDown.distance;

            if (!able)
            transform.position = new Vector2(transform.position.x + ((gameObject.transform.lossyScale.x == -1) ? -0.1f : 0.1f), transform.position.y + distanceToUp + 0.2f);
        }
    }

    private RaycastHit2D IsNearScalableSurface()
    {
        return Physics2D.Linecast(scaleCheck.position, scaleCheck.position + ((gameObject.transform.lossyScale.x == -1) ? Vector3.left : Vector3.right) * scalingDistance, climbableLayer);
    }

    private RaycastHit2D CanScaleSurface()
    {
        return Physics2D.Linecast(scaleHelp.position, scaleHelp.position + ((gameObject.transform.lossyScale.x == -1) ? Vector3.left : Vector3.right) * scalingDistance, climbableLayer);
    }

    private RaycastHit2D IsInTheGround()
    {
        return Physics2D.Linecast(scaleDebug.position, scaleDebug.position, climbableLayer);
    }

    private void Phasing()
    {
        if (IsNearClimbableSurface() && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            transform.position = new Vector2(transform.position.x + ((gameObject.transform.lossyScale.x == -1) ? -1f : 1f), transform.position.y);
        }
    }

    private void GroundCheck()
    {
        foreach (Transform groundCheck in groundCheckers)
        {
            grounded = Physics2D.OverlapCircle(groundCheck.position, checkerRadius, ground);
            if (grounded)
                break;
        }

        if (grounded)
            bonusJumpsLeft = bonusJumps;
    }

    private void LadderCheck()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)) 
        {
            if (IsNearLadder(out Vector2 hitPoint))
            {
                var tpos = ladderMap.WorldToCell(hitPoint) + new Vector3(0.5f, 0.5f, 0f);
                transform.position = tpos + ladderMapOffset;
                rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
                onLadder = true;
                rb.gravityScale = 0;
                movementSpeed = 0;
                bonusJumpsLeft = bonusJumps;
            }
        }
    }

    public void GetOffLadder()
    {
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        onLadder = false;
        rb.gravityScale = 1f;
        if (!crouched)
        movementSpeed = originSpeed;
    }

    private bool IsNearLadder(out Vector2 hitPoint)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position - ((gameObject.transform.lossyScale.x == -1) ? Vector3.left : Vector3.right) * ladderDistance, transform.position + ((gameObject.transform.lossyScale.x == -1) ? Vector3.left : Vector3.right) * ladderDistance, ladderLayer);

        if (hit.collider != null)
        {
            hitPoint = hit.point;
            ladderMap = hit.collider.GetComponent<Tilemap>();
            ladderMapOffset = ladderMap.transform.position;
            return true;
        }

        hitPoint = Vector2.zero;
        return false;
    }

    private void Crouching()
    {
        if (Input.GetKeyDown(KeyCode.S) && !crouched && !onLadder && !attacking)
        {
            crouched = true;
            movementSpeed = crouchingSpeed;
            jumpForce = jumpForce - 1;
            animator.SetBool("Crouched", true);
        }
        else if ((Input.GetKeyUp(KeyCode.S) && crouched) || attacking)
        {
            crouched = false;

            if (!attacking)
            {
                movementSpeed = originSpeed;
                jumpForce = jumpForce + 1;
            }
            animator.SetBool("Crouched", false);
        }
    }

    private void CrouchMovement()
    {

    }

    private void Blocking()
    {
        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) && !blocking && !parrying)
        {
            blocking = true;
            movementSpeed = 0;
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetBool("Blocking", true);
            parryingTimer = originParryingTimer;
            interuptRoll = true;

            StartCoroutine(Parry());
            StartCoroutine(StopParry());
            Roll();
        }
        else if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) && parrying)
        {
            parryRepeat++;
        }
        else if ((Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl)) && blocking && parryingTimer < 0)
        {
            blocking = false;
            movementSpeed = originSpeed;
            animator.SetBool("Blocking", false);
        }
        else if ((Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl)) && blocking && parryingTimer >= 0)
        {
            animator.SetBool("Blocking", false);
            //animator parrying true
            blocking = false;
            StartCoroutine(Parry());
            Debug.Log("PARRY!");
        }
    }

    public IEnumerator StopParry()
    {
        yield return new WaitForSeconds(startParryTimer);

        if (blocking)
        parryStop = true;
    }

    public IEnumerator Parry()
    {
        parrying = true;
        parrySprite.SetActive(true);
        parryDuration = originParryDuration;
        yield return new WaitUntil(() => parryInterupt || parryDuration < 0 || parryStop);

        if (parryInterupt)
            StartCoroutine(AknowledgeParry());

        if (parryInterupt && parryRepeat > 0 && !parryStop)
        {
            parryRepeat--;
            StartCoroutine(Parry());
            yield break;
        }

        parryInterupt = false;
        parrying = false;
        parrySprite.SetActive(false);

        if (!parryStop)
        movementSpeed = originSpeed;

        parryStop = false;
    }

    public IEnumerator AknowledgeParry()
    {
        parried = true;
        yield return new WaitForSeconds(0.1f);
        parried = false;
    }

    public void WeaponRecoil(float knockback)
    {
        rb.velocity += new Vector2(knockback * (gameObject.transform.localScale.x == -1 ? -1f : 1f), 0);
    }

    public void TakeKnockback(float additiveForce)
    {
        /*knockbackForce += additiveForce;
        decayKnockback = true; */
        beingKnocked = true;
        rb.AddForce(new Vector2(additiveForce, 0), ForceMode2D.Impulse);
    }

    public void KnockbackDecay()
    {
        if (knockbackForce > 0)
        {
            knockbackForce -= knockbackDecayRate * Time.deltaTime;
            knockbackForce = Mathf.Max(0f, knockbackForce);
        }
        else
        {
            knockbackForce += knockbackDecayRate * Time.deltaTime;
            knockbackForce = Mathf.Min(0f, knockbackForce);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * climbDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.back * climbDistance);

        Debug.DrawLine(transform.position, transform.position + Vector3.left * scalingDistance, Color.blue);
        Debug.DrawLine(transform.position, transform.position + Vector3.right * scalingDistance, Color.blue);
    }
}