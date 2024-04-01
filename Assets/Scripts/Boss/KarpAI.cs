using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarpAI : MonoBehaviour
{
    [Header("AI")]
    public RaycastHit2D playerIsInRange;
    public RaycastHit2D playerIsVisible;
    public RaycastHit2D playerIsBehind;
    public RaycastHit2D playerIsInProximity;
    public RaycastHit2D playerIsInCloseProximity;
    public RaycastHit2D playerIsInReach;
    public EnemyAI AI;
    public EnemyPatrol patrol;
    public EnemyAttackPing ping;
    public EnemyHealth health;
    public Rigidbody2D rb;
    public bool attacking;
    private bool switched = false;
    public float standartPingTimer;
    public bool Inert;
    public bool canTurnAttack;
    public int canTurnCounter;
    public float attackTurnCooldown;
    private float originAttackTurnCooldown;

    [Header("Phases")]
    public bool comboPlus;
    public bool parryPlus;
    public bool slashPlus;
    public bool shrapnelPlus;
    public bool pillarsPlus;
    public int currentPhase = 1;
    public int comboValue;
    public int parryValue;
    public int slashValue;
    public int shrapnelValue;
    public int pillarsValue;
    public int[] phaseOneValues;
    public int[] phaseTwoValues;
    public int[] phaseThreeValues;
    public int resetCounter;
    public float attackCooldown;
    public float attakCooldownReset;
    public float pillarCooldown;
    public float pillarCooldownReset;
    public float parryCooldown;
    public float parryCooldownReset;
    public bool firstAttack;

    [Header("Combat Mode")]
    public float combatModeTimeLimit;
    public float combatModeTimeLeft;
    public bool combatMode;

    [Header("Speed")]
    public float walkingSpeed;
    public float pursuitSpeed;
    public float chargeSpeed;

    [Header("Combo")]
    public bool comboPursuit;
    public bool comboing;
    public float comboInitialWindUp;
    public float comboCooldown;
    private float originComboCooldown;
    public bool puruitAttack;
    public float attack1WindUp;
    public float attack1Recovery;
    public float attack2WindUp;
    public float attack2Recovery;
    public float attack3WindUp;
    public float attack3Recovery;
    public float attack2PursuitSpeed;
    public float attack3PursuitSpeed;

    public EnemyAttackMelee attack1;
    public EnemyAttackMelee attack2;
    public EnemyAttackMelee attack3;
    public Enum.attackType comboAttackType;
    public float attack1Duration;
    public float attack2Duration;
    public float attack3Duration;


    public GameObject slapAttack;
    public float slapDuration;
    public float slapAttackDuration;
    public Transform slapPos;
    //attacks
    //timings
    //combo reset timer
    //seqeunce control
    //turning around ai

    [Header("Slash")]
    public bool slashing;
    public bool inSlashPosition;
    public Transform slashPointL;
    public Transform slashPointR;
    public bool gettingInPosition;
    public EnemyPatrol.direction slashPointDirection;
    public float slashWindUp;
    public float slashRecovery;
    public GameObject slashAttack;
    public float slashDuration;
    public float slashAttackDuration;
    public Transform slashPos;
    public int slashDamage;
    public float timingBetweenSlashes;

    [Header("Parry")]
    public bool parrying;
    private bool parrying2;
    public bool guardUp;
    public float guardTimer;
    public float parryWindup;
    public float guardRecovery;
    public bool guarding;
    public float ripostWindUp;
    public float ripostRecovery;
    private float ripostRecoveryTimer;
    public bool riposting;
    public GameObject ripostAttack;
    public float ripostDuration;
    public float ripostAttackDuration;
    public Transform ripostPos;
    public int ripostDamage;
    public float parryStunDuration;

    [Header("Shrapnel")]
    public Transform shrapnelPointL;
    public Transform shrapnelPointM;
    public Transform shrapnelPointR;
    public int shrapnelNumber;
    public int shrapnelPlusNumber;
    public bool shrapneling;
    public float shrapnelWindUP;
    public float shrapnelSuspence;
    public float shrapnelPreSuspence;
    public float shrapnelRecovery;
    public GameObject shrapnelProjectile;
    public int shrapnelDamage;
    public float shrapnelSpeed;
    public float shrapnelAngle;
    public Transform midPoint;
    public GameObject shrapnelSlamAttack;
    public int shrapnelSlamDamage;
    public float shrapnelSlamDuration;
    public float shrapnelSlamAttackDuration;
    public Transform shrapnelSlamPoint;

    [Header("Pillars")]
    public bool pillaring;
    public float pillarWindUp;
    public float pillarRecovery;
    public List<GameObject> pillarsWave1;
    public List<GameObject> pillarsWave2;
    public float timingBetweenPillars1;
    public float timingBetweenPillars2;
    public int pillarDamage;
    public float pillarLifeTime;
    public float pillarWarning;
    public float pillarWaveCooldown;


    [Header("Debug")]
    public bool isBehind;

    public GameObject player;
    public float pushBack = 2f;
    private float pushBackOrigin;
    public bool canPushBack = true;
    public Cutscene scene;

    private void Start()
    {
        AI = gameObject.GetComponent<EnemyAI>();
        patrol = gameObject.GetComponent<EnemyPatrol>();
        ping = gameObject.GetComponent<EnemyAttackPing>();
        health = gameObject.GetComponent<EnemyHealth>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.Find("Gamer");

        originComboCooldown = comboCooldown;
        originAttackTurnCooldown = attackTurnCooldown;
        pushBackOrigin = pushBack;
        ripostRecoveryTimer = ripostRecovery;
        Inert = true;
        firstAttack = true;

        ResetProbability();
    }

    void Update()
    {
        if (Inert)
            return;

        playerIsInRange = AI.playerIsInRange;
        playerIsVisible = AI.playerIsVisible;
        playerIsBehind = AI.playerIsBehind;
        playerIsInProximity = AI.playerIsInProximity;
        playerIsInCloseProximity = AI.playerIsInCloseProximity;
        playerIsInReach = AI.playerIsInReach;

        attacking = comboing || parrying || slashing || shrapneling || pillaring;

        comboCooldown -= Time.deltaTime;

        attackCooldown -= Time.deltaTime;

        pillarCooldown -= Time.deltaTime;

        parryCooldown -= Time.deltaTime;

        attackTurnCooldown -= Time.deltaTime;

        ripostRecoveryTimer -= Time.deltaTime;

        patrol.combatMode = combatMode;


        if (playerIsInCloseProximity)
        {
            if (canPushBack)
            {
                // player.GetComponent<PlayerMovement>().TakeKnockback(player.transform.position.x < transform.position.x ? -pushBack : pushBack);
            }
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
            NewPhase();

        AIControl();
    }

    private void AIControl()
    {
        isBehind = false;

        if (canTurnAttack && playerIsBehind && !playerIsVisible && canTurnCounter > 0 && attackTurnCooldown < 0)
        {
            patrol.switchDirection();
            switched = true;
            canTurnCounter--;
            attackTurnCooldown = originAttackTurnCooldown;
        }

        if (!playerIsBehind)
            switched = false;

        if (gettingInPosition)
        {
            switch (slashPointDirection)
            {
                case EnemyPatrol.direction.left:
                    if (transform.position.x <= slashPointL.position.x)
                    {
                        gettingInPosition = false;
                        inSlashPosition = true;
                    }
                    break;
                case EnemyPatrol.direction.right:
                    if (transform.position.x >= slashPointR.position.x)
                    {
                        gettingInPosition = false;
                        inSlashPosition = true;
                    }
                    break;
            }
        }

        if (guarding && AI.parried && !riposting)
        {
            riposting = true;
            AI.parried = false;
            guarding = false;
            health.guarding = false;
            canTurnAttack = false;
            StartCoroutine(Ripost());
        }

        if (attacking)
            return;


        if (playerIsVisible)
        {
            patrol.movementSpeed = pursuitSpeed;
        }
        else if (playerIsBehind && !switched)
        {
            patrol.switchDirection();
            switched = true;
        }


        if (!playerIsBehind)
            switched = false;


        if (playerIsInCloseProximity && playerIsVisible)
        {
            patrol.movementSpeed = -walkingSpeed;
            //patrol.movementSpeed = 0;
        }


        else if (playerIsInReach)
        {
            patrol.movementSpeed = 0;
        }

        if (comboPursuit || attackCooldown > 0)
            return;

        int attackValues = comboValue + parryValue + slashValue + shrapnelValue + pillarsValue;
        int attackChoose = Random.Range(1, attackValues + 1);


        if ((attackChoose <= attackValues - (parryValue + slashValue + shrapnelValue + pillarsValue)) || firstAttack)
        {
            comboPursuit = true;

            firstAttack = false;

            StartCoroutine(Combo());

            resetCounter++;
            comboValue--;
        }
        else if (attackChoose <= attackValues - (slashValue + shrapnelValue + pillarsValue) && parryCooldown < 0)
        {
            parrying = true;

            StartCoroutine(Parry());

            resetCounter++;
            parryValue--;
        }
        else if (attackChoose <= attackValues - (shrapnelValue + pillarsValue))
        {
            slashing = true;

            StartCoroutine(Slash());

            resetCounter++;
            slashValue--;
        }
        else if (attackChoose <= attackValues - (pillarsValue))
        {
            shrapneling = true;

            StartCoroutine(Shrapnel());

            resetCounter++;
            shrapnelValue--;
        }
        else if (attackChoose <= attackValues && pillarCooldown < 0)
        {
            pillaring = true;

            StartCoroutine(Pillars());

            resetCounter++;
            pillarsValue--;
        }


        if (currentPhase == 1 && resetCounter > attackValues)
        {
            resetCounter = 0;
            ResetProbability();
        }
        else if (resetCounter > attackValues)
        {
            resetCounter = 0;
            ResetProbability();
        }
    }

    public void ResetProbability()
    {
        switch (currentPhase)
        {
            case 1:
                comboValue = phaseOneValues[0];
                parryValue = phaseOneValues[1];
                slashValue = phaseOneValues[2];
                shrapnelValue = phaseOneValues[3];
                pillarsValue = phaseOneValues[4];
                break;
            case 2:
                comboValue = phaseTwoValues[0];
                parryValue = phaseTwoValues[1];
                slashValue = phaseTwoValues[2];
                shrapnelValue = phaseTwoValues[3];
                pillarsValue = phaseTwoValues[4];
                break;
            case 3:
                comboValue = phaseThreeValues[0];
                parryValue = phaseThreeValues[1];
                slashValue = phaseThreeValues[2];
                shrapnelValue = phaseThreeValues[3];
                pillarsValue = phaseThreeValues[4];
                break;
        }
    }

    public void NewPhase()
    {
        currentPhase++;
        ResetProbability();

        switch (currentPhase)
        {
            case 2:
                comboPlus = true;
                break;
            case 3:
                parryPlus = true;
                slashPlus = true;
                shrapnelPlus = true;
                pillarsPlus = true;
                break;
        }
    }

    public IEnumerator Combo()
    {
        yield return new WaitUntil(() => playerIsInReach);

        patrol.movementSpeed = 0;
        comboing = true;
        comboPursuit = false;

        yield return new WaitForSeconds(comboInitialWindUp);

        ping.Ping(Enum.attackType.yellow);
        yield return new WaitForSeconds(attack1WindUp); 
        ping.Ping(Enum.attackType.yellow);

        yield return new WaitForSeconds(standartPingTimer - attack1WindUp);

        attack1.Attack(comboAttackType);
        //Instantiate(slapAttack).GetComponent<EnemyAttack>().setParameters(20, slapDuration, slapDuration, Enum.attackType.yellow, slapPos, gameObject.transform);
        yield return new WaitForSeconds(attack1Duration);
        attack1.UnAttack();

        yield return new WaitForSeconds(attack1WindUp - attack1Duration);

        attack1.Attack(comboAttackType);
        //Instantiate(slapAttack).GetComponent<EnemyAttack>().setParameters(20, slapDuration, slapDuration, Enum.attackType.yellow, slapPos, gameObject.transform);
        yield return new WaitForSeconds(attack1Duration);
        attack1.UnAttack();

        yield return new WaitForSeconds(attack1Recovery - attack1Duration);

        canTurnAttack = true;
        canTurnCounter = 1;

        patrol.movementSpeed = attack2PursuitSpeed;
        yield return new WaitUntil(() => playerIsInProximity);
        patrol.movementSpeed = 0;


        yield return new WaitForSeconds(attack2WindUp);
        ping.Ping(Enum.attackType.yellow);

        canTurnAttack = false;

        yield return new WaitForSeconds(standartPingTimer);

        attack2.Attack(comboAttackType);
        //Instantiate(slapAttack).GetComponent<EnemyAttack>().setParameters(20, slapDuration, slapDuration, Enum.attackType.yellow, slapPos, gameObject.transform);
        yield return new WaitForSeconds(attack2Duration);
        attack2.UnAttack();

        yield return new WaitForSeconds(attack2Recovery - attack2Duration);

        if (!comboPlus)
        {
            attackCooldown = attakCooldownReset;
            comboing = false;
            comboCooldown = originComboCooldown;
            yield break;
        }

        canTurnAttack = true;
        canTurnCounter = 1;

        patrol.movementSpeed = attack3PursuitSpeed;
        yield return new WaitUntil(() => playerIsInProximity);
        patrol.movementSpeed = 0;

        yield return new WaitForSeconds(attack3WindUp);
        ping.Ping(Enum.attackType.yellow);

        //puruit false
        canTurnAttack = false;

        yield return new WaitForSeconds(standartPingTimer);

        attack3.Attack(comboAttackType);
        //(slapAttack).GetComponent<EnemyAttack>().setParameters(20, slapDuration, slapDuration, Enum.attackType.yellow, slapPos, gameObject.transform);
        yield return new WaitForSeconds(attack3Duration);
        attack3.UnAttack();

        yield return new WaitForSeconds(attack3Recovery - attack3Duration);

        attackCooldown = attakCooldownReset;
        comboing = false;
        comboCooldown = originComboCooldown;
    }

    public IEnumerator Slash()
    {
        patrol.movementSpeed = 0;

        if (Vector2.Distance(transform.position, slashPointL.position) < Vector2.Distance(transform.position, slashPointR.position))
        {
            Transform startPoint = slashPointL;
            Transform endPoint = slashPointR;
            slashPointDirection = EnemyPatrol.direction.left;
            if (patrol.facing == EnemyPatrol.direction.right)
                patrol.switchDirection();
        }
        else
        {
            Transform startPoint = slashPointR;
            Transform endPoint = slashPointL;
            slashPointDirection = EnemyPatrol.direction.right;
            if (patrol.facing == EnemyPatrol.direction.left)
                patrol.switchDirection();
        }

        gettingInPosition = true;
        patrol.movementSpeed = pursuitSpeed;

        yield return new WaitUntil(() => inSlashPosition);

        patrol.movementSpeed = 0;
        patrol.switchDirection();
        inSlashPosition = false;

        yield return new WaitForSeconds(slashWindUp - standartPingTimer);

        ping.Ping(Enum.attackType.yellow);

        if (slashPlus)
        {
            yield return new WaitForSeconds(timingBetweenSlashes);
            ping.Ping(Enum.attackType.yellow);
        }

        yield return new WaitForSeconds(standartPingTimer);

        Instantiate(slashAttack).GetComponent<EnemyAttack>().setParameters(slashDamage, slashDuration, slashAttackDuration, Enum.attackType.yellow, slashPos, gameObject.transform);

        switch (slashPointDirection)
        {
            case EnemyPatrol.direction.left:
                if (transform.position.x <= slashPointR.position.x)
                    transform.position = new Vector2(slashPointR.position.x, transform.position.y);
                break;
            case EnemyPatrol.direction.right:
                if (transform.position.x >= slashPointL.position.x)
                    transform.position = new Vector2(slashPointL.position.x, transform.position.y);
                break;
        }

        if (!slashPlus)
        {
            yield return new WaitForSeconds(slashRecovery);

            attackCooldown = attakCooldownReset;
            slashing = false;

            yield break;
        }

        yield return new WaitForSeconds(timingBetweenSlashes);

        patrol.switchDirection();

        Instantiate(slashAttack).GetComponent<EnemyAttack>().setParameters(slashDamage, slashDuration, slashAttackDuration, Enum.attackType.yellow, slashPos, gameObject.transform);

        switch (slashPointDirection)
        {
            case EnemyPatrol.direction.right:
                if (transform.position.x <= slashPointR.position.x)
                    transform.position = new Vector2(slashPointR.position.x, transform.position.y);
                break;
            case EnemyPatrol.direction.left:
                if (transform.position.x >= slashPointL.position.x)
                    transform.position = new Vector2(slashPointL.position.x, transform.position.y);
                break;
        }

        yield return new WaitForSeconds(slashRecovery);

        attackCooldown = attakCooldownReset;
        slashing = false;
    }

    public IEnumerator Parry()
    {
        patrol.movementSpeed = 0;

        yield return new WaitForSeconds(parryWindup);
        ping.Ping(Enum.attackType.blue);
        guarding = true;
        health.guarding = true;
        canTurnAttack = true;
        canTurnCounter = 4;
        yield return new WaitForSeconds(guardTimer);
        guarding = false;
        health.guarding = false;
        canTurnAttack = false;
        yield return new WaitForSeconds(guardRecovery);

        if (!riposting)
        {
            parrying = false;
            attackCooldown = attakCooldownReset;
            parryCooldown = parryCooldownReset;
        }
    }

    public IEnumerator Ripost()
    {
        yield return new WaitForSeconds(ripostWindUp);

        //Instantiate(ripostAttack).GetComponent<EnemyAttack>().setParameters(ripostDamage, ripostDuration, ripostAttackDuration, Enum.attackType.blue, ripostPos, gameObject.transform);
        attack3.Attack(comboAttackType);
        yield return new WaitForSeconds(attack3Duration);
        attack3.UnAttack();

        ripostRecoveryTimer = ripostRecovery - attack3Duration;

        yield return new WaitUntil(() => player.GetComponent<PlayerMovement>().parried || ripostRecoveryTimer < 0);

        if (player.GetComponent<PlayerMovement>().parried)
        {
            Debug.Log("SHOULD STUN");
            if (parryPlus)
            {
                yield return new WaitForSeconds(ripostWindUp);
                attack3.Attack(comboAttackType);
                yield return new WaitForSeconds(attack3Duration);
                attack3.UnAttack();

                if (player.GetComponent<PlayerMovement>().parried)
                {
                    gameObject.GetComponent<StatusEffectDisplay>().AddEffect(Enum.weaponEffect.stun);
                    yield return new WaitForSeconds(parryStunDuration);
                    gameObject.GetComponent<StatusEffectDisplay>().RemoveEffect(Enum.weaponEffect.stun);
                }

            }
            else
            {
                Debug.Log("STUN");
                gameObject.GetComponent<StatusEffectDisplay>().AddEffect(Enum.weaponEffect.stun);
                yield return new WaitForSeconds(parryStunDuration);
                gameObject.GetComponent<StatusEffectDisplay>().RemoveEffect(Enum.weaponEffect.stun);
            }
        }

        attackCooldown = attakCooldownReset;
        parryCooldown = parryCooldownReset;
        riposting = false;
        parrying = false;
    }

    public IEnumerator Shrapnel()
    {
        patrol.movementSpeed = 0;
        yield return new WaitForSeconds(shrapnelWindUP);

        rb.gravityScale = 0;
        patrol.canTurn = false;

        float tilt = shrapnelAngle;
        float tiltMove = (180f - (tilt * 2)) / (shrapnelPlus ? shrapnelPlusNumber : shrapnelNumber);

        int shrapPos = Random.Range(1, 4);

        switch (shrapPos)
        {
            case 1:
                transform.position = shrapnelPointL.position;
                tilt += 60;
                if (patrol.facing == EnemyPatrol.direction.left)
                    patrol.switchDirection();
                break;
            case 2:
                transform.position = shrapnelPointM.position;
                break;
            case 3:
                transform.position = shrapnelPointR.position;
                tilt -= 60;
                if (patrol.facing == EnemyPatrol.direction.right)
                    patrol.switchDirection();
                break;
        }

        tilt += ((180f - (tilt * 2)) / (shrapnelPlus ? shrapnelPlusNumber : shrapnelNumber)) / 2;

        yield return new WaitForSeconds(shrapnelPreSuspence);
        ping.Ping(Enum.attackType.yellow);

        if (shrapnelPlus)
        {
            yield return new WaitForSeconds(shrapnelSuspence);
            ping.Ping(Enum.attackType.green);
        }

        yield return new WaitForSeconds(standartPingTimer);


        for (int i = 0; i < (shrapnelPlus ? shrapnelPlusNumber : shrapnelNumber); i++)
        {
            Instantiate(shrapnelProjectile, transform.position, shrapnelPointM.rotation = Quaternion.Euler(0, 0, tilt)).GetComponent<ProjectileAttackDirectional>().SetParameters(shrapnelSpeed, shrapnelDamage);
            tilt += tiltMove;
        }

        shrapnelPointM.rotation = Quaternion.Euler(0, 0, 0);

        yield return new WaitForSeconds(shrapnelSuspence);
        rb.gravityScale = 1;
        patrol.canTurn = true;

        switch (shrapPos)
        {
            case 1:
                transform.position = slashPointL.position;
                break;
            case 2:
                transform.position = midPoint.position;
                break;
            case 3:
                transform.position = slashPointR.position;
                break;
        }


        if (!shrapnelPlus)
        {
            yield return new WaitForSeconds(shrapnelRecovery);
            attackCooldown = attakCooldownReset;
            shrapneling = false;
            yield break;
        }

        Instantiate(shrapnelSlamAttack).GetComponent<EnemyAttack>().setParameters(shrapnelSlamDamage, shrapnelSlamDuration, shrapnelSlamAttackDuration, Enum.attackType.green, shrapnelSlamPoint, gameObject.transform);

        yield return new WaitForSeconds(shrapnelRecovery);
        attackCooldown = attakCooldownReset;
        shrapneling = false;
    }

    public IEnumerator Pillars()
    {
        patrol.movementSpeed = 0;

        yield return new WaitForSeconds(pillarWindUp);

        StartCoroutine(PillarsFinish());

        int flip = Random.Range(0, 2);

        if (flip == 0)
        {
            foreach(GameObject pillar in pillarsWave1)
            {
                pillar.GetComponent<HitScanArea>().setParameters(pillarDamage, pillarLifeTime, Enum.attackType.green, pillarWarning, standartPingTimer);
                yield return new WaitForSeconds(timingBetweenPillars1);
            }
        }
        else
        {
            for (int i = pillarsWave1.Count - 1; i >= 0; i--)
            {
                GameObject pillar = pillarsWave1[i];
                pillar.GetComponent<HitScanArea>().setParameters(pillarDamage, pillarLifeTime, Enum.attackType.green, pillarWarning, standartPingTimer);
                yield return new WaitForSeconds(timingBetweenPillars1);
            }
        }

        if (!pillarsPlus)
            yield break;

        yield return new WaitForSeconds(pillarWaveCooldown);

        if (flip == 1)
        {
            foreach (GameObject pillar in pillarsWave2)
            {
                pillar.GetComponent<HitScanArea>().setParameters(pillarDamage, pillarLifeTime, Enum.attackType.green, pillarWarning, standartPingTimer);
                yield return new WaitForSeconds(timingBetweenPillars2);
            }
        }
        else
        {
            for (int i = pillarsWave2.Count - 1; i >= 0; i--)
            {
                GameObject pillar = pillarsWave2[i];
                pillar.GetComponent<HitScanArea>().setParameters(pillarDamage, pillarLifeTime, Enum.attackType.green, pillarWarning, standartPingTimer);
                yield return new WaitForSeconds(timingBetweenPillars2);
            }
        }
    }

    public IEnumerator PillarsFinish()
    {
        yield return new WaitForSeconds(pillarRecovery);

        pillarCooldown = pillarCooldownReset;
        pillaring = false;
    }

    public void RestartCombatTimer()
    {
        combatModeTimeLeft = combatModeTimeLimit;
        combatMode = true;
    }

    public void OnDestroy()
    {
        scene.OpenDoors();
    }
}