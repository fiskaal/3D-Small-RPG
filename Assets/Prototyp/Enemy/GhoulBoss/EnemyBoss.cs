/*
using System.Collections;
using System.Collections.Generic;
using CartoonFX;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject hitVFX1;
    [SerializeField] GameObject ragdoll;

    [Header("Combat")]
    [SerializeField] float health = 3;
    [SerializeField] float attackCD = 3f;       //basic attack
    [SerializeField] float attackCD1 = 5f;      //jump attack
    [SerializeField] float attackRange = 1f;
    [SerializeField] float aggroRange = 4f;
    [SerializeField] private GameObject preAttackWarningPrefab;
    private bool isAttacking;
    private double attackingTime;
    private float attackTimePassed;

    [Header("Loot")]
    [SerializeField] GameObject[] lootItems;
    [SerializeField] Vector2Int[] lootQuantities;

    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    float timePassed;
    private float timePassed1;
    float newDestinationCD = 0.5f;

    private bool dead;
    private float timePassedAfterDeath;

    private bool firstTimeSpotted;
    private float screamTimePassed = 0;

    private HealthSystem playerHealthSystem;

    //HpBar
    [SerializeField] private EnemyHpBar _enemyHpBar;
    
    //ocko projectile
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
        timePassed = attackCD;
        
        //hpBar
        _enemyHpBar.SetMaxHP(health);
            
        //ocko projectile
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

        if (isAttacking)
        {
            attackTimePassed += Time.deltaTime; // Accumulate the elapsed time

            if (attackTimePassed >= attackingTime)
            {
                isAttacking = false;
                animator.SetBool("isAttacking", false);
                attackTimePassed = 0f; // Reset the elapsed time
            }
        }
        
        
        
        if (timePassed >= attackCD)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
            {
                if (playerHealthSystem.health > 0)
                {
                    animator.applyRootMotion = true;
                    animator.SetTrigger("attack");
                    animator.SetBool("isAttacking", true);
                    Instantiate(preAttackWarningPrefab, transform);
                    timePassed = 0;

                    isAttacking = true;
                    attackingTime = animator.GetCurrentAnimatorClipInfo(0).Length;

                    if (_ockoProjectile != null)
                    {
                        _ockoProjectile.FireProjectile(player.transform.position);
                    }
                }
            }
        }
        timePassed += Time.deltaTime;
        
        if (timePassed1 >= attackCD1)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= attackRange + 1)
            {
                if (playerHealthSystem.health > 0)
                {
                    animator.applyRootMotion = true;
                    animator.SetTrigger("attackJump");
                    animator.SetBool("isAttacking", true);
                    Instantiate(preAttackWarningPrefab, transform);
                    timePassed1 = 0;

                    isAttacking = true;
                    attackingTime = animator.GetCurrentAnimatorClipInfo(0).Length;


                    if (_ockoProjectile != null)
                    {
                        _ockoProjectile.FireProjectile(player.transform.position);
                    }
                }
            }
        }
        timePassed1 += Time.deltaTime;

        if (newDestinationCD <= 0 && Vector3.Distance(player.transform.position, transform.position) <= aggroRange && !dead && !isAttacking)
        {
            newDestinationCD = 0.5f;
            agent.SetDestination(player.transform.position);
        }
        newDestinationCD -= Time.deltaTime;

        if (Vector3.Distance(player.transform.position, transform.position) <= aggroRange && !dead && !isAttacking)
        {
            // Calculate the direction from the enemy to the player
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0f; // Set the Y component to zero to avoid rotation in the Y-axis

            if (directionToPlayer != Vector3.zero)
            {
                // Rotate the enemy to face the player's direction
                transform.rotation = Quaternion.LookRotation(directionToPlayer);
            }
        }

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

    void Die()
    {
        DropLoot();
        Destroy(this.gameObject);
    }

    void DropLoot()
    {
        for (int i = 0; i < lootItems.Length; i++)
        {
            int quantity = Random.Range(lootQuantities[i].x, lootQuantities[i].y + 1); // Random quantity within the specified range
            for (int j = 0; j < quantity; j++)
            {
                Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(1f, 3f);
                Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y); // Offset only in X and Z dimensions

                Instantiate(lootItems[i], spawnPosition + Vector3.up, Quaternion.identity);
            }
        }
    }


    public void TakeDamage(float damageAmount)
    {
        if (!dead)
        {
            health -= damageAmount;
            animator.applyRootMotion = true;
            animator.SetTrigger("damage");
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
        GetComponentInChildren<EnemyDamageDealer>().StartDealDamage();
    }

    public void EndDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().EndDealDamage();
    }

    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);

        GameObject hit1 = Instantiate(hitVFX1, hitPosition, Quaternion.identity);
        Destroy(hit1, 3f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}
*/
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
    public float fallImpactAttackRadius;
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
        
        if (!isAttacking)
        {
            UpdateDestination();
            UpdateRotation(5);
            
            Vector3 directionToPlayer = player.transform.position - transform.position;
            // Calculate the angle between the enemy's forward direction and the direction to the player
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            // Define a threshold angle, for instance, 5 degrees
            float angleThreshold = 5f;

            if (angleToPlayer < angleThreshold)
            {
                UpdateAttackState(basAttackCd, basicAttackRange, ref timePassedAttackCD);
                //UpdateAttackState(jumpAttackCd, basicAttackRange + 1, ref timePassedAttackCD1);
                UpdateAttackState(stompAttackCd, basicAttackRange, ref timePassedAttackCD2);
        
                UpdateAttackState(spitAttackCd, spitAttackRangeMax, ref spitAttackTimePassed);
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
                        Attack("attack");
                    }
                    else if (attackCooldown == stompAttackCd)
                    {
                        isIdle = false;
                        currentAttackRadius = legAttackRadius;
                        currentAttackDamage = legAttackDamage;
                        Attack("attackStomp");
                    }
                    else if (attackCooldown == spitAttackCd)
                    {
                        isIdle = false;
                        currentAttackRadius = 0;
                        currentAttackDamage = 0;
                        Attack("attackSpit");
                        OckoProjectile projectile = GetComponent<OckoProjectile>();
                        projectile.FireProjectile(player.transform.position, transform);
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

            if (directionToPlayer != Vector3.zero)
            {
                //transform.rotation = Quaternion.LookRotation(directionToPlayer);
                
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), rotationSpeed * Time.deltaTime);
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
            _damagePopUpGenerator.CreatePopUp(hit.position, damageAmount.ToString());
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
