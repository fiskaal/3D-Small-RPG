using System.Collections;
using System.Collections.Generic;
using CartoonFX;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] bool hasMoreAttacks;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject hitVFX1;
    [SerializeField] GameObject ragdoll;

    [Header("Combat")]
    [SerializeField] float health = 3;
    [SerializeField] float attackCD = 3f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float aggroRange = 4f;
    [SerializeField] private GameObject preAttackWarningPrefab;
    [SerializeField] float rotationSpeed = 4f;
    [SerializeField] private float angleThreshold = 5f;

    [Header("ocko adittional attack chance")]
    [SerializeField]private float attackChance = 0.5f;

    [Header("Loot")]
    [SerializeField] GameObject[] lootItems;
    [SerializeField] Vector2Int[] lootQuantities;

    public GameObject player;
    NavMeshAgent agent;
    Animator animator;
    float timePassed;
    float newDestinationCD = 0.5f;

    private bool dead;
    private float timePassedAfterDeath;

    private bool firstTimeSpotted;
    private float screamTimePassed = 0;
    private bool enemyIsInRange;
    public bool isAttacking;

    private float encounterTimer = 0f;
    public bool firstEncounter = false;
    public bool pursuingPlayer;
    private Vector3 originalPosition;
    [SerializeField]private float timeSinceLastSighting = 0f;
    [SerializeField]private float chillingDelay = 10f;
    [SerializeField]private float playerNotInRangeGiveUpTime = 10f;

    private HealthSystem playerHealthSystem;
    [SerializeField] private DamagePopUpGenerator _damagePopUpGenerator;
    private CapsuleCollider _collider;
    
    //keeps distance after attack
    public bool stepBackAfterAttack = false; // Toggle for stepping back after attack
    public float safeDistance = 5f; // Distance to maintain after stepping back

    // Track the last time the enemy attacked
    private float lastAttackTime;
    private bool isSteppingBack;

    //HpBar
    [SerializeField] private EnemyHpBar _enemyHpBar;
    
    //ocko projectile
    private OckoProjectile _ockoProjectile;

    private float agentSpeedRegular;
    private float agentSpeedPatrol;
    
    //quick attack
    private float quickAttacktimer;
    public bool hasQuickAttack = false;

    public bool bigTreeWhipAttackKnocksDown = false;


    //path patroling
    public bool patrolPath = false;
    public Transform[] waypoints; // Array to hold the waypoints
    public float patrolSpeed = 3.5f;
    public float waypointWaitTime = 2f;

    private int currentWaypointIndex = 0;
    private bool isWaiting;
    
    //audio
    public AudioScript audioScript;
    
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealthSystem = FindObjectOfType<HealthSystem>();

        originalPosition = transform.position;
        
        animator.applyRootMotion = false;
        dead = false;
        timePassedAfterDeath = 0f;
        firstTimeSpotted = true;
        timePassed = attackCD;

        if (GetComponent<AudioScript>() != null)
        {
            audioScript = GetComponent<AudioScript>();
        }

        //hpBar
        _enemyHpBar.SetMaxHP(health);
        _enemyHpBar.SetHP(health);


        _damagePopUpGenerator = FindObjectOfType<DamagePopUpGenerator>();
        damageDealers = GetComponentsInChildren<EnemyDamageDealer>();    
        
        //ocko projectile
        if (GetComponent<OckoProjectile>() != null)
        {
            _ockoProjectile = GetComponent<OckoProjectile>();
        }

        agentSpeedRegular = agent.speed;
        agentSpeedPatrol = agent.speed / 2;
        
        //patrol
        if (patrolPath)
        {
            MoveToNextWaypoint();
        }
        else
        {
            animator.SetTrigger("chilling");
        }

        _collider = GetComponent<CapsuleCollider>();

        maxHp = health;
        currentHp = health;
    }

    void Update()
    {
        //float normalizedSpeed = Mathf.InverseLerp(agentSpeedPatrol, agentSpeedRegular, agent.speed);
        //float currentSpeed = Mathf.Lerp(0.5f, 1.0f, normalizedSpeed);
        
        float currentSpeed = agent.velocity.magnitude/agent.speed;
        animator.SetFloat("speed", currentSpeed);
        
        // Check if the agent's speed is zero
        if (agent.velocity.magnitude == 0f) 
        {
            currentSpeed = 0f; // Set currentSpeed to 0 if the speed is zero
        }
        
        animator.SetFloat("speed", currentSpeed);

        if (player == null)
        {
            return;
        }

        if (Vector3.Distance(player.transform.position, transform.position) <= aggroRange && !isAttacking)
        {
            agent.speed = agentSpeedRegular;
            pursuingPlayer = true;
            
            enemyIsInRange = true;
            timeSinceLastSighting = 0f; // Reset timer since the player is spotted
            
            if (!firstEncounter)
            {
                animator.SetTrigger("enemySpotted");

                if (encounterTimer >= animator.GetCurrentAnimatorClipInfo(0).Length)
                {
                    firstEncounter = true;
                    encounterTimer = 0f;
                }
                else
                {
                    encounterTimer += Time.deltaTime;
                }
            }
        }
        else
        {
            enemyIsInRange = false;

            if (firstEncounter)
            {
                // Increment timer if the player is not spotted
                timeSinceLastSighting += Time.deltaTime;

                // If the timer reaches the delay, trigger chilling animation and return to original position
                if (timeSinceLastSighting >= chillingDelay)
                {
                    // Check if the enemy reached its original position
                    if (Vector3.Distance(transform.position, originalPosition) < 3f && !patrolPath)
                    {
                        // Trigger chilling animation when back to original position
                        animator.SetTrigger("chilling");
                        timeSinceLastSighting = 0f; // Reset timer after triggering "chilling"
                        firstEncounter = false;
                    }
                    else if (!patrolPath)
                    {
                        agent.SetDestination(originalPosition);
                        animator.ResetTrigger("enemySpotted");
                    }
                    else
                    {
                        MoveToNextWaypoint();
                        firstEncounter = false;
                        timeSinceLastSighting = 0f; // Reset timer after triggering "chilling"
                    }
                }
                
                if (timeSinceLastSighting >= playerNotInRangeGiveUpTime)
                {
                    pursuingPlayer = false;
                }
                else
                {
                    pursuingPlayer = true;
                }
            }
            
            
        }

        if (patrolPath && !firstEncounter)
        {
            float allowedDistance = agent.stoppingDistance + 0.1f;
            if (!agent.pathPending && agent.remainingDistance < allowedDistance && !isWaiting)
            {
                StartCoroutine(WaitAtWaypoint());
            }
        }

        if (firstEncounter)
        {

            //attack
            // Calculate the direction from the enemy to the player
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0f; // Set the Y component to zero to avoid rotation in the Y-axis
            // Calculate the angle between the enemy's forward direction and the direction to the player
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            // Define a threshold angle, for instance, 5 degrees

            
                if (timePassed >= attackCD && !dead)
                {
                    
                        if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
                        {
                            if (playerHealthSystem.health > 0)
                            {
                                if (angleToPlayer < angleThreshold)
                                {

                                    isAttacking = true;
                                    animator.applyRootMotion = true;
                                    agent.ResetPath();

                                    if (hasMoreAttacks)
                                    {
                                        //choose random one attack
                                        float randomValue = Random.value;
                                        if (randomValue > 0.5f)
                                        {
                                            animator.SetTrigger("attack");
                                        }
                                        else
                                        {
                                            animator.SetTrigger("attack1");
                                        }
                                    }
                                    else
                                    {
                                        animator.SetTrigger("attack");
                                    }

                                    Instantiate(preAttackWarningPrefab, transform);
                                    timePassed = 0;

                                    if (_ockoProjectile != null)
                                    {
                                        //_ockoProjectile.FireProjectile(player.transform.position, transform);

                                        float randomValue = Random.value;
                                        attackChance = 0.5f;
                                        if (randomValue <= attackChance)
                                        {
                                            StartCoroutine(TriggerAttackAfterDelay());
                                        }
                                    }
                                }
                                else if (quickAttacktimer >= 2f && hasQuickAttack)
                                {
                                    isAttacking = true;
                                    animator.applyRootMotion = true;
                                    agent.ResetPath();
                                    QuickAttack(directionToPlayer);
                                    quickAttacktimer = 0f;
                                    timePassed = 0f;
                                }

                                quickAttacktimer += Time.deltaTime;
                            }
                        }
                    

                }
            

            if (stepBackAfterAttack)
            {
                if (timePassed < attackCD && !dead && !isAttacking)
                {
                    StepBack();
                }
                else
                {
                    isSteppingBack = false;
                }
            }

            timePassed += Time.deltaTime;

            if (newDestinationCD <= 0 && pursuingPlayer && !dead && !isSteppingBack)
            {
                newDestinationCD = 0.5f;
                agent.SetDestination(player.transform.position);
            }

            newDestinationCD -= Time.deltaTime;

            if (Vector3.Distance(player.transform.position, transform.position) <= aggroRange && !dead && !isSteppingBack)
            {
                // Calculate the direction from the enemy to the player
                //its done up there

                if (directionToPlayer != Vector3.zero)
                {
                    // Rotate the enemy to face the player's direction
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(directionToPlayer), rotationSpeed * Time.deltaTime);

                }
            }
            //enemy roam
            if (!dead && !enemyIsInRange && !pursuingPlayer && !isSteppingBack)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    Vector3 point;
                    if (RandomPoint(transform.position, aggroRange, out point))
                    {
                        Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                        agent.SetDestination(point);
                        
                        agent.speed = agentSpeedPatrol;
                    }
                }
            }
        }
    }

    private void QuickAttack(Vector3 directionToPlayer)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(directionToPlayer), 1000 * Time.deltaTime);
        animator.SetTrigger("attack");
    }
    
    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waypointWaitTime);
        isWaiting = false;
        MoveToNextWaypoint();
    }
    
    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.speed = agentSpeedPatrol;
    }

    IEnumerator TriggerAttackAfterDelay()
    {
        float delay = 0.5f; // Time delay before triggering the attack
        
        yield return new WaitForSeconds(delay);

        // Trigger the attack after the delay
        Instantiate(preAttackWarningPrefab, transform);
        animator.SetTrigger("attack");
        //_ockoProjectile.FireProjectile(player.transform.position, transform);
    }

    
    //enemy roam
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 ranndomPoint = center + Random.insideUnitSphere * range; //random point in sphere
        NavMeshHit hit;
        
        if (NavMesh.SamplePosition(ranndomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    public void pursuePlayer()
    {
        agent.ResetPath();
        agent.SetDestination(player.transform.position);
        timeSinceLastSighting = 0f;
        pursuingPlayer = true;
        agent.speed = agentSpeedRegular;
    }

    public void StepBack()
    {
        // Move the enemy away from the player to maintain a safe distance
        Vector3 directionToPlayer = transform.position - player.transform.position;
        Vector3 safePosition = transform.position + directionToPlayer.normalized * (safeDistance + attackRange);
        agent.SetDestination(safePosition);
        
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(safePosition), rotationSpeed * Time.deltaTime);
        isSteppingBack = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }

    void DropLoot()
    {
        for (int i = 0; i < lootItems.Length; i++)
        {
            int quantity = Random.Range(lootQuantities[i].x, lootQuantities[i].y + 1);

            for (int j = 0; j < quantity; j++)
            {
                Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(1f, 3f);
                Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);

                // Use NavMesh.SamplePosition to find a valid position on the NavMesh
                if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
                {
                    spawnPosition = hit.position + Vector3.up;
                }

                Instantiate(lootItems[i], spawnPosition, Quaternion.identity);
            }
        }
    }


    public GameObject deathVFX;

    public float maxHp;
    public float currentHp;
    
    public void TakeDamage(float damageAmount, Transform hit)
    {
        if (!dead)
        {
            _damagePopUpGenerator.CreatePopUp(hit.position, damageAmount.ToString(), Color.white);
            if (audioScript != null)
            {
                audioScript.PlayHit();
            }

            health -= damageAmount;
            currentHp = health;
            animator.applyRootMotion = true;
            if (!isAttacking)
            {
                animator.SetTrigger("damage");
            }
            _enemyHpBar.SetHP(health);

            pursuePlayer();
            
            if (health <= 0)
            {
                StartCoroutine(Death());
                
                if (deathVFX != null)
                {
                    GameObject death = Instantiate(deathVFX, transform);
                    death.transform.SetParent(null);
                }
            }
            
            if (!firstEncounter)
            {
                firstEncounter = true;
                //animator.SetTrigger("enemySpotted");
            }
        }
    }

    IEnumerator Death()
    {
        dead = true;
        animator.SetTrigger("death");
        DropLoot();
        
        _enemyHpBar.SetHealthBarInvisible();
        _enemyHpBar.gameObject.SetActive(false);
        Outline outline =  GetComponentInChildren<Outline>();
        outline.SetOutlineWidth(0f);
        outline.enabled = false;
        _collider.enabled = false;
        
        yield return new WaitForSeconds(3f);
        
        Die();
    }
    
    void Die()
    {
        Destroy(this.gameObject);
    }


/*
    public void StartDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().StartDealDamage();
    }

    public void EndDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().EndDealDamage();
    }
*/

    public EnemyDamageDealer[] damageDealers;

    // Assign the EnemyDamageDealer instances to the array slots in the Inspector

    public void StartDealDamage()
    {
        foreach (var dealer in damageDealers)
        {
            dealer.StartDealDamage();
        }
    }

    public void EndDealDamage()
    {
        foreach (var dealer in damageDealers)
        {
            dealer.EndDealDamage();
            OnAttackAnimationEnd();
        }
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

    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
    }
}
