using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCombo : MonoBehaviour
{
    public float[] attackDelays;
    public float[] attackDurations;
    public float[] attackRecoils;

    public float[] attackWindUps;
    public float[] attackRecoveries;

    public GameObject[] attacks;

    public int currentAttack = 0;
    public float currentDelay = 0;
    public float currentRecovery;

    public float comboTimer;
    private float comboTimerOriginal;

    public bool pickedUp = false;
    public bool activlyAttacking = false;

    public float requestTimer;
    public float requestTimerOriginal;
    public bool requestOnlyOneAttack;

    private GameObject player;
    private PlayerMovement move;

    public Enum.weaponEffect effect;
    public float effectTimer;
    public int effectStrength;

    public bool comboing;
    public bool canAttack;
    public bool windingUp;

    public float tempWindUp;
    void Start()
    {
        requestTimerOriginal = requestTimer;
        comboTimerOriginal = comboTimer;
        player = GameObject.Find("Gamer");
        move = player.GetComponent<PlayerMovement>();
        canAttack = true;
    }

    void Update()
    {
        requestTimer -= Time.deltaTime;
        currentDelay -= Time.deltaTime;
        currentRecovery -= Time.deltaTime;
        comboTimer -= Time.deltaTime;
        tempWindUp -= Time.deltaTime;

        if (currentDelay < 0)
            comboing = false;

        if (!activlyAttacking && canAttack && requestTimer > 0 && !move.onLadder)
        {
            StartCoroutine(Attack());

            if (requestOnlyOneAttack)
                requestTimer = 0;
        }

        if (comboTimer < 0)
        {
            currentAttack = 0;
        }
    }

    public void AttackRequest()
    {
        requestTimer = requestTimerOriginal;
    }

    public void Attackz()
    {
        activlyAttacking = true;
        comboTimer = comboTimerOriginal;
     //   requestTimer = 0;
        attacks[currentAttack].gameObject.SetActive(true);
        player.GetComponent<PlayerMovement>().WeaponRecoil(attackRecoils[currentAttack]);
        StartCoroutine(AttackDuration());
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        windingUp = true;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, player.GetComponent<Rigidbody2D>().velocity.y);
        move.movementSpeed = 0;
        tempWindUp = attackWindUps[currentAttack];
        move.interuptRoll = true;

        //yield return new WaitForSeconds(attackWindUps[currentAttack]);

        yield return new WaitUntil(() => (move.rolling && !move.interuptRoll)|| move.blocking || move.parrying || tempWindUp < 0);

        if (move.rolling || move.blocking || move.parrying)
        {
            canAttack = true;
            windingUp = false;
            yield break;
        }

        windingUp = false;
        player.GetComponent<PlayerMovement>().interuptRoll = false;
        activlyAttacking = true;
        comboTimer = comboTimerOriginal;
        attacks[currentAttack].gameObject.SetActive(true);
        player.GetComponent<PlayerMovement>().WeaponRecoil(attackRecoils[currentAttack]);

        yield return new WaitForSeconds(attackDurations[currentAttack]);

        attacks[currentAttack].gameObject.SetActive(false);
        activlyAttacking = false;

        yield return new WaitForSeconds(attackRecoveries[currentAttack]);

        currentAttack = (currentAttack + 1 == attacks.Length) ? 0 : currentAttack + 1;
        move.movementSpeed = move.originSpeed;
        canAttack = true;

    }

    private IEnumerator AttackDuration()
    {
        yield return new WaitForSeconds(attackDurations[currentAttack]);
        attacks[currentAttack].gameObject.SetActive(false);
        activlyAttacking = false;
        currentDelay = attackDelays[currentAttack];
        currentAttack = (currentAttack + 1 == attacks.Length) ? 0 : currentAttack + 1;
    }
}
