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
    [SerializeField] private float aggroRange = 4f;
    [SerializeField] private GameObject preAttackWarningPrefab;

    private bool isAttacking;
    private float attackingTime;
    private float attackTimePassed;
    private float timePassedAttackCD;
    private float jumpAttackTimePassed;
    private float timePassedAttackCD2;

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
    private Rigidbody rb;

    private BossDamageDealer _bossDamageDealer;
    
    //jump attack
    [FormerlySerializedAs("attackCD1")] [SerializeField] private float jumpAttackCd = 8f;      // Jump attack cooldown
    [SerializeField] private float jumpAttackRadius = 15f; // Jump attack radius
    private bool isJumpAttacking;
    private bool isFalling;
    private Vector3 targetPosition;
    private bool isGrounded;
    private bool fallImpactVFXPlayed;
    
    // Health bar
    [SerializeField] private EnemyHpBar _enemyHpBar;

    // Ocko projectile
    private OckoProjectile _ockoProjectile;

    void Start()
    {
        isGrounded = true;
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
        jumpAttackTimePassed = jumpAttackCd; //jump 

        _bossDamageDealer = GetComponentInChildren<BossDamageDealer>();

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        
        // Set max HP for the health bar
        _enemyHpBar.SetMaxHP(health);

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

        UpdateAttackState(basAttackCd, basicAttackRange, ref timePassedAttackCD);
        //UpdateAttackState(jumpAttackCd, basicAttackRange + 1, ref timePassedAttackCD1);
        UpdateAttackState(stompAttackCd, basicAttackRange, ref timePassedAttackCD2);
        
        // Check for conditions to initiate the jump attack
        if (!isJumpAttacking && !isFalling && jumpAttackTimePassed >= jumpAttackCd 
            && Vector3.Distance(player.transform.position, transform.position) <= jumpAttackRadius && isIdle)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0f;

            if (directionToPlayer != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToPlayer);
            }

            rb.isKinematic = false;
            isIdle = false;
            agent.Stop();
            agent.enabled = false;
            isJumpAttacking = true;
            StartCoroutine(ExecuteJumpAttack());
        }
        jumpAttackTimePassed += Time.deltaTime;

        //jump attack fall impact 
        // Cast a ray downwards to check if the boss has hit the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f) && isFalling)
        {
            if (hit.collider.CompareTag("Ground")) // Assuming the ground has a tag named "Ground"
            {
                isGrounded = true;
                if (!fallImpactVFXPlayed)
                {
                    Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

                    // Calculate the position 0.2 units closer to the player from the boss's position
                    Vector3 newPosition = transform.position + directionToPlayer * 0.5f;

                    // Instantiate the effect at the new position
                    InstantiateAndDestroyVFX(fallImpactVFX, newPosition);                   
                    fallImpactVFXPlayed = true;
                }
                // Boss has hit the ground
            }
        }
        else
        {
            isGrounded = false;
            // Boss is not touching the ground
        }
        
        UpdateDestination();

        UpdateRotation();

        HandleDeath();
    }
    
    
    private IEnumerator ExecuteJumpAttack()
    {
        
        currentAttackDamage = fallImpactAttackDamage;
        currentAttackRadius = fallImpactAttackRadius;
        
        fallImpactVFXPlayed = false;
        agent.enabled = false;

        isAttacking = true;
        // Jump Animation
        animator.SetTrigger("Jump");

        // Wait for the jump animation to complete
        yield return new WaitForSeconds(3.39f);
        if (rb != null)
        {
            // Apply a force to make the boss jump
            rb.AddForce(Vector3.up * 50, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody component not found on the boss GameObject.");
            yield break; // Exit the coroutine if Rigidbody is not found
        }
        
        // Play the fall animation and initiate the fall
        animator.SetTrigger("Fall");
        isFalling = true;
        
        yield return new WaitForSeconds(1); // wait for falling to end
        StartFallAttack();
    }

// Called from the animation event at the end of the Fall animation
    public void StartFallAttack() 
    {
        StartDealDamage();
        // Set the boss's position to the target position (player's position)
        // Calculate the position to fall
        Vector3 offset = new Vector3(0f, 0f, 2f); // Adjust the Z-axis offset as needed

        targetPosition = player.transform.position + offset;
        targetPosition.y = transform.position.y;
        transform.position = targetPosition;
        
        rb.AddForce(Vector3.down * 100, ForceMode.Impulse);


        // Play the fall-attack animation
        animator.SetTrigger("FallAttack");
        
        // Deal damage or perform other actions as the boss falls
        // Implement the necessary logic here
    }

// Called from the animation event at the end of the Fall-Attack animation
    public void EndFallAttack()
    {
        // Reset the flags and cooldown for the next jump attack
        EndDealDamage();
        isJumpAttacking = false;
        isFalling = false;
        jumpAttackTimePassed = 0f;
        isAttacking = false;
        rb.isKinematic = true;
        agent.enabled = true;
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
                    animator.applyRootMotion = true;

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

                    Instantiate(preAttackWarningPrefab, transform);

                    isAttacking = true;
                    attackingTime = animator.GetCurrentAnimatorClipInfo(0).Length;

                    if (_ockoProjectile != null)
                    {
                        _ockoProjectile.FireProjectile(player.transform.position);
                    }
                }
                timePassed = 0; // Reset the attack cooldown
            }
        }
        timePassed += Time.deltaTime;
    }

    private void Attack(string triggerName)
    {
        animator.SetTrigger(triggerName);
        animator.SetBool("isAttacking", true);
    }

    public void ResetIsAttacking()
    {
        animator.SetBool("isAttacking", false);
        isAttacking = false;
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

    private void UpdateRotation()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= aggroRange && !dead && !isAttacking)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0f;

            if (directionToPlayer != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToPlayer);
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

    public void TakeDamage(float damageAmount)
    {
        isIdle = false;
        if (!dead)
        {
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
