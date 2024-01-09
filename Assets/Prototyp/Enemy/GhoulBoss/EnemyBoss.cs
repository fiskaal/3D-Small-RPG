
using System.Collections;
using System.Collections.Generic;
using CartoonFX;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class EnemyBoss : MonoBehaviour
{
    [Header("Damage")]
    public float legAttackDamage;
    public float handAttackDamage;
    public float fallImpactAttackDamage;
    public float currentAttackDamage;

    [Header("Attack Hit Radius")] 
    public float legAttackRadius;
    public float handAttackRadius;
    public float jumpImpactAttackRadius = 5;
    public float currentAttackRadius;
    

    
    [Header("Visual Effects")]
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject hitVFX1;
    [SerializeField] private GameObject toxicSmoke;
    [SerializeField] private GameObject fallImpactVFX;
    



    [Header("Combat")]
    [SerializeField] private float health = 3;
    [FormerlySerializedAs("attackCD")] [SerializeField] private float basAttackCd = 3f;       // Basic attack cooldown
    [FormerlySerializedAs("stumpAttackCd")] [SerializeField] private float stompAttackCd = 4;
    [FormerlySerializedAs("attackRange")] [SerializeField] private float basicAttackRange = 1f;
    [SerializeField] private float spitAttackCd = 3f;
    [SerializeField] private float spitAttackRangeMin = 5f;
    [SerializeField] private float spitAttackRangeMax = 7f;
    [SerializeField] private float jumpAttackRange = 2f;
    [SerializeField] private float jumpAttackCD = 20f;
    [SerializeField] private float aggroRange = 4f;
    [SerializeField] private GameObject preAttackWarningPrefab;

    private bool isAttacking;
    private float attackingTime;
    private float attackTimePassed;
    private float timePassedAttackCD;
    private float jumpAttackTimePassed;
    private float timePassedAttackCD2;
    private float spitAttackTimePassed;

    [Header("RageMode")] 
    [SerializeField] private float rageModeBonusHealth = 20f;
    [SerializeField] private bool rageMode = false;
    [SerializeField] private bool rage = false;
    [SerializeField] private ParticleSystem rageSmoke;
    [SerializeField] private float halfOfHealth;
    // color change
    public Material bossMaterial;
    public Color startColor = Color.white; // FFFFFF
    public Color endColor = new Color(1f, 0.467f, 0.467f); // FF7777
    public float transitionDuration = 5.0f; // Duration for the color transition in seconds

    private float startTime;
    
    public Color particleStartColor = Color.white; // Initial color of the particle system
    public float particleTransitionDuration = 3.0f; // Duration for particle color transition in seconds
    public float emissionRate = 20f; // Starting emission rate

    private float particleStartTime;


    

    [Header("Loot")]
    [SerializeField] private GameObject[] lootItems;
    [SerializeField] private Vector2Int[] lootQuantities;

    public GameObject player;  
    private NavMeshAgent agent;
    private Animator animator;
    private float timePassed;
    private float timePassed1;
    private float newDestinationCD = 0.5f;

    private bool dead;
    private float timePassedAfterDeath;

    private bool firstTimeSpotted;
    private float screamTimePassed = 0;

    private HealthSystem playerHealthSystem;

    private BossDamageDealer _bossDamageDealer;

    private DamagePopUpGenerator _damagePopUpGenerator;
    
    
    float targetScale = 1.5f;
    float startScale = 1f;
    float scaleTransitionDuration = 5f; // Duration in seconds
    float scaleStartTime;
    
    // Health bar
    [SerializeField] private EnemyHpBar _enemyHpBar;

    // Ocko projectile
    private OckoProjectile _ockoProjectile;

    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealthSystem = player.GetComponent<HealthSystem>();

        animator.applyRootMotion = false;
        dead = false;
        timePassedAfterDeath = 0f;
        firstTimeSpotted = true;
        timePassed = basAttackCd;
        timePassedAttackCD = basAttackCd; //basic
        spitAttackTimePassed = spitAttackCd;
        jumpAttackTimePassed = jumpAttackCD;
        
        _bossDamageDealer = GetComponentInChildren<BossDamageDealer>();

        bossMaterial.color = startColor;
        halfOfHealth = health / 2;
        startTime = Time.time;
        particleStartTime = Time.time;


        scaleStartTime = Time.time;
        
        // Set max HP for the health bar
        _enemyHpBar.SetMaxHP(health);
        
        _damagePopUpGenerator = FindObjectOfType<DamagePopUpGenerator>();
        
        // Ocko projectile
        if (GetComponent<OckoProjectile>() != null)
        {
            _ockoProjectile = GetComponent<OckoProjectile>();
        }
    }

    void Update()
    {
        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);

        if (player == null)
        {
            return;
        }

        if (health <= halfOfHealth && !rage && !isAttacking)
        {
            rage = true;
            agent.SetDestination(transform.position);
            agent.ResetPath();
            RageModeActive();
        }

        if (rageMode)
        {
            // Material color change
            float lerpFactor = Mathf.Clamp01((Time.time - startTime) / transitionDuration);
            Color lerpedColor = Color.Lerp(startColor, endColor, lerpFactor);
            bossMaterial.color = lerpedColor;
            
            StartCoroutine(ScaleOverTime());

            // Particle system color change
            float particleLerpFactor = Mathf.Clamp01((Time.time - particleStartTime) / particleTransitionDuration);
            Color particleLerpedColor = Color.Lerp(particleStartColor, endColor, particleLerpFactor);

            // Modify particle system color
            var mainModule = rageSmoke.main;
            mainModule.startColor = particleLerpedColor;

            // Modify emission rate
            var emissionModule = rageSmoke.emission;
            emissionModule.rateOverTime = emissionRate;
        }
      
        UpdateCoolDown(ref timePassedAttackCD);
        UpdateCoolDown(ref timePassedAttackCD2);
        UpdateCoolDown(ref spitAttackTimePassed);
        UpdateCoolDown(ref jumpAttackTimePassed);

        
        if (!isAttacking)
        {
            UpdateDestination();
            UpdateRotation(5);
            
            Vector3 directionToPlayer = player.transform.position - transform.position;
            // Calculate the angle between the enemy's forward direction and the direction to the player
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            // Define a threshold angle, for instance, 5 degrees
            float angleThreshold = 10f;

            if (angleToPlayer < angleThreshold)
            {
                UpdateAttackState(basAttackCd, basicAttackRange, ref timePassedAttackCD);
                //UpdateAttackState(jumpAttackCd, basicAttackRange + 1, ref timePassedAttackCD1);
                UpdateAttackState(stompAttackCd, basicAttackRange, ref timePassedAttackCD2);
        
                UpdateAttackState(spitAttackCd, spitAttackRangeMax, ref spitAttackTimePassed);

                if (rageMode)
                {
                    UpdateAttackState(jumpAttackCD, jumpAttackRange, ref jumpAttackTimePassed);
                }
            }
        }
        
        HandleDeath();
    }

    private void UpdateCoolDown(ref float timePassed)
    {
        timePassed += Time.deltaTime;
    }
    
    IEnumerator ScaleOverTime()
    {
        while (Time.time < startTime + transitionDuration)
        {
            float scaleLerpFactor = Mathf.Clamp01((Time.time - startTime) / transitionDuration);
            float lerpedScale = Mathf.Lerp(startScale, targetScale, scaleLerpFactor);
            transform.localScale = new Vector3(lerpedScale, lerpedScale, lerpedScale);
            yield return null; // Wait for the next frame
        }

        // Ensure reaching the target scale at the end of the transition
        transform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }
    
    private void UpdateAttackState(float attackCooldown, float currentAttackRange, ref float timePassed)
    {
        if (timePassed >= attackCooldown && Vector3.Distance(player.transform.position, transform.position) <= currentAttackRange)
        {
            // Check if the enemy is not already attacking
            if (!isAttacking)
            {
                if (playerHealthSystem.health > 0)
                {

                    if (attackCooldown == basAttackCd)
                    {
                        isIdle = false;
                        currentAttackDamage = handAttackDamage;
                        currentAttackRadius = handAttackRadius;
                        _bossDamageDealer.heavyDamage = false;
                        Attack("attack");
                        agent.ResetPath();
                    }
                    else if (attackCooldown == jumpAttackCD)
                    {
                        isIdle = false;
                        currentAttackRadius = jumpImpactAttackRadius;
                        currentAttackDamage = fallImpactAttackDamage;
                        _bossDamageDealer.heavyDamage = true;
                        Attack("jumpAttack");
                        agent.ResetPath();
                    }
                    else if (attackCooldown == stompAttackCd)
                    {
                        isIdle = false;
                        currentAttackRadius = legAttackRadius;
                        currentAttackDamage = legAttackDamage;
                        _bossDamageDealer.heavyDamage = false;
                        float randomValue = Random.value;
                        if (randomValue > 0.5f)
                        {
                            Attack("attackStomp");
                        }
                        else
                        {
                            Attack("attackStompFlipped");
                        }
                        agent.ResetPath();
                    }
                    else if (attackCooldown == spitAttackCd)
                    {
                        isIdle = false;
                        currentAttackRadius = 0;
                        currentAttackDamage = 0;
                        _bossDamageDealer.heavyDamage = false;
                        Attack("attackSpit");
                        agent.ResetPath();
                    }
                    else if (attackCooldown == jumpAttackCD)
                    {
                        isIdle = false;
                        currentAttackRadius = jumpImpactAttackRadius;
                        currentAttackDamage = fallImpactAttackDamage;
                        _bossDamageDealer.heavyDamage = true;
                        Attack("jumpAttack");
                        agent.ResetPath();
                    }

                    Instantiate(preAttackWarningPrefab, transform);

                    isAttacking = true;
                    attackingTime = animator.GetCurrentAnimatorClipInfo(0).Length;
                }
                timePassed = 0; // Reset the attack cooldown
            }
        }
    }
    
    private void Attack(string triggerName)
    {
        animator.applyRootMotion = true;
        animator.SetTrigger(triggerName);
        animator.SetBool("isAttacking", true);
    }

    public void ResetIsAttacking()
    {
        animator.SetBool("isAttacking", false);
        isAttacking = false;
        animator.applyRootMotion = false;
    }

    public void RageModeActive()
    {
        health = health + rageModeBonusHealth;
        isIdle = false;
        currentAttackDamage = handAttackDamage;
        currentAttackRadius = handAttackRadius;
        _bossDamageDealer.heavyDamage = false;
        Attack("roar");
        isAttacking = true;
        attackingTime = animator.GetCurrentAnimatorClipInfo(0).Length;
    }

    public void RageModeParticleChange()
    {
        if (rageSmoke != null)
        {
            rageMode = true;
        }
    }

    private void UpdateDestination()
    {
        if (agent != null)
        {
            if (newDestinationCD <= 0 &&
                Vector3.Distance(player.transform.position, transform.position) <= aggroRange && !dead && !isAttacking)
            {
                newDestinationCD = 0.5f;
                agent.SetDestination(player.transform.position);
            }

            newDestinationCD -= Time.deltaTime;
        }
    }

    private void UpdateRotation(float rotationSpeed)
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= aggroRange && !dead)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0f;
            directionToPlayer = directionToPlayer.normalized;
            

            if (directionToPlayer != Vector3.zero)
            {
                //transform.rotation = Quaternion.LookRotation(directionToPlayer);
                
                transform.rotation = Quaternion.Slerp(transform.rotation.normalized, Quaternion.LookRotation(directionToPlayer), rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void HandleDeath()
    {
        if (dead)
        {
            if (timePassedAfterDeath >= 3f)
            {
                Die();
            }
            timePassedAfterDeath += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }

    private void Die()
    {
        DropLoot();
        Destroy(gameObject);
    }

    private void DropLoot()
    {
        for (int i = 0; i < lootItems.Length; i++)
        {
            int quantity = Random.Range(lootQuantities[i].x, lootQuantities[i].y + 1);
            for (int j = 0; j < quantity; j++)
            {
                Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(1f, 3f);
                Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);

                Instantiate(lootItems[i], spawnPosition + Vector3.up, Quaternion.identity);
            }
        }
    }

    public void TakeDamage(float damageAmount, Transform hit)
    {
        isIdle = false;
        if (!dead)
        {
            _damagePopUpGenerator.CreatePopUp(hit.position, damageAmount.ToString(), Color.white);
            health -= damageAmount;
            animator.applyRootMotion = true;
            if (isIdle)
            {
                animator.SetTrigger("damage");
            }
            _enemyHpBar.SetHP(health);

            if (health <= 0)
            {
                animator.SetTrigger("death");
                animator.SetBool("dead", true);
                dead = true;
            }
        }
    }

    public void StartDealDamage()
    {
        _bossDamageDealer.ChangeDamage(currentAttackDamage, currentAttackRadius);
        _bossDamageDealer.StartDealDamage();
        _bossDamageDealer.GetComponent<Animation>().Play("DamageDealerAttack");

        //GetComponentInChildren<EnemyDamageDealer>().StartDealDamage();
    }

    public void EndDealDamage()
    {
        _bossDamageDealer.EndDealDamage();
        _bossDamageDealer.GetComponent<Animation>().Stop("DamageDealerAttack");

        //GetComponentInChildren<EnemyDamageDealer>().EndDealDamage();
    }

    public void HitVFX(Vector3 hitPosition)
    {
        InstantiateAndDestroyVFX(hitVFX, hitPosition);
        InstantiateAndDestroyVFX(hitVFX1, hitPosition);
    }

    private void InstantiateAndDestroyVFX(GameObject vfxPrefab, Vector3 position)
    {
        GameObject vfx = Instantiate(vfxPrefab, position, Quaternion.identity);
        Destroy(vfx, 3f);
    }

    private bool isIdle;
    public void SetIdle()
    {
        isIdle = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, basicAttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}
