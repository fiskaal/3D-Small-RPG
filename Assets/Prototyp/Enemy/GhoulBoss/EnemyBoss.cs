
using System.Collections;
using System.Collections.Generic;
using CartoonFX;
using UnityEngine;
using UnityEngine.AI;
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
    [SerializeField] private GameObject ragdoll;
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
    

    [Header("Loot")]
    [SerializeField] private GameObject[] lootItems;
    [SerializeField] private Vector2Int[] lootQuantities;

    private GameObject player;
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
                
                UpdateAttackState(jumpAttackCD, jumpAttackRange, ref jumpAttackTimePassed);
            }
        }
        
        HandleDeath();
    }

    private void UpdateCoolDown(ref float timePassed)
    {
        timePassed += Time.deltaTime;
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
                        OckoProjectile projectile = GetComponent<OckoProjectile>();
                        projectile.FireProjectile(player.transform.position, transform);
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
