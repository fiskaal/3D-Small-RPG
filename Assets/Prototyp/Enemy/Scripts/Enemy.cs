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

    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    float timePassed;
    float newDestinationCD = 0.5f;

    private bool dead;
    private float timePassedAfterDeath;

    private bool firstTimeSpotted;
    private float screamTimePassed = 0;
    private bool enemyIsInRange;
    private bool isAttacking;

    public bool firstEncounter = false;
    private Vector3 originalPosition;
    [SerializeField]private float timeSinceLastSighting = 0f;
    [SerializeField]private float chillingDelay = 10f;

    private HealthSystem playerHealthSystem;
    [SerializeField] private DamagePopUpGenerator _damagePopUpGenerator;

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

        animator.SetTrigger("chilling");
        originalPosition = transform.position;
        
        animator.applyRootMotion = false;
        dead = false;
        timePassedAfterDeath = 0f;
        firstTimeSpotted = true;
        timePassed = attackCD;
        
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
    }

    void Update()
    {
        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);

        if (player == null)
        {
            return;
        }

        if (Vector3.Distance(player.transform.position, transform.position) <= aggroRange)
        {
            enemyIsInRange = true;
            timeSinceLastSighting = 0f; // Reset timer since the player is spotted
            
            if (!firstEncounter)
            {
                firstEncounter = true;
                animator.SetTrigger("enemySpotted");
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
                    if (Vector3.Distance(transform.position, originalPosition) < 2f)
                    {
                        // Trigger chilling animation when back to original position
                        animator.SetTrigger("chilling");
                        timeSinceLastSighting = 0f; // Reset timer after triggering "chilling"
                        firstEncounter = false;
                    }
                    else
                    {
                        agent.SetDestination(originalPosition);
                    }
                }
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

            if (angleToPlayer < angleThreshold)
            {
                if (timePassed >= attackCD && !dead)
                {
                    if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
                    {
                        if (playerHealthSystem.health > 0)
                        {
                            isAttacking = true;
                            animator.applyRootMotion = true;

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
                                _ockoProjectile.FireProjectile(player.transform.position, transform);

                                float randomValue = Random.value;
                                attackChance = 0.5f;
                                if (randomValue <= attackChance)
                                {
                                    StartCoroutine(TriggerAttackAfterDelay());
                                }
                            }
                        }
                    }
                }
            }

            timePassed += Time.deltaTime;

            if (newDestinationCD <= 0 &&
                Vector3.Distance(player.transform.position, transform.position) <= aggroRange && !dead)
            {
                newDestinationCD = 0.5f;
                agent.SetDestination(player.transform.position);
            }

            newDestinationCD -= Time.deltaTime;

            if (Vector3.Distance(player.transform.position, transform.position) <= aggroRange && !dead)
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
            if (!dead && !enemyIsInRange)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    Vector3 point;
                    if (RandomPoint(transform.position, aggroRange, out point))
                    {
                        Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                        agent.SetDestination(point);
                    }
                }
            }
        }
        
        if (dead)
        {
            if (timePassedAfterDeath >= 1f)
            {
                Die();
            }

            timePassedAfterDeath += Time.deltaTime;
        }
    }
    
    IEnumerator TriggerAttackAfterDelay()
    {
        float delay = 0.5f; // Time delay before triggering the attack

        yield return new WaitForSeconds(delay);

        // Trigger the attack after the delay
        Instantiate(preAttackWarningPrefab, transform);
        animator.SetTrigger("attack");
        _ockoProjectile.FireProjectile(player.transform.position, transform);
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
    
    public void TakeDamage(float damageAmount, Transform hit)
    {
        if (!dead)
        {
            _damagePopUpGenerator.CreatePopUp(hit.position, damageAmount.ToString(), Color.white);
            
            health -= damageAmount;
            animator.applyRootMotion = true;
            if (!isAttacking)
            {
                animator.SetTrigger("damage");
            }
            _enemyHpBar.SetHP(health);

            agent.SetDestination(player.transform.position);
            
            if (health <= 0)
            {
                animator.SetTrigger("death");
                dead = true;

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
