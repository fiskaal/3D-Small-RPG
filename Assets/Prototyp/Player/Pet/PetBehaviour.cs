using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PetBehaviour : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent agent;
    public float playerRange = 5f;
    public float attackRange = 2f;
    public float attackCooldown = 10f;
    public float angleThreshold = 5f;
    public LayerMask enemyLayer; // Layer for other enemies
    
    private Animator anim;
    private bool isAttacking;
    private float lastAttackTime;
    private bool enemyLocked;

    private float idleActionTimer;
    private bool isChilling;

    private GameObject closestEnemy;
    float closestDistance = Mathf.Infinity;

    private DamageDealer _damageDealer;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        _damageDealer = GetComponentInChildren<DamageDealer>();
    }

    void Update()
    {
        float speed = agent.velocity.magnitude/agent.speed;
        anim.SetFloat("speed", speed);
        
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            
            if (distanceToPlayer > playerRange)
            {
                randomDestination = GetRandomPointAroundPlayer();
                agent.SetDestination(randomDestination);
                // Calculate the direction from the enemy to the player
                Vector3 directionRandomPoint = player.transform.position - transform.position;
                directionRandomPoint.y = 0f; // Set the Y component to zero to avoid rotation in the Y-axis
                transform.rotation = Quaternion.Slerp(transform.rotation.normalized, Quaternion.LookRotation(directionRandomPoint), 50 * Time.deltaTime);
                isChilling = false;
            }
            else
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerRange, enemyLayer);

                foreach (var collider in hitColliders)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);

                    if (hitColliders.Length <= 1)
                    {
                        closestDistance = distanceToEnemy;
                    }

                    if (distanceToEnemy <= playerRange / 2)
                    {
                        if (distanceToEnemy <= closestDistance)
                        {
                            closestDistance = distanceToEnemy;
                            closestEnemy = collider.gameObject;
                        }
                    }
                }


                if (closestEnemy.gameObject != gameObject && closestEnemy != null) // Ensure the pet doesn't attack itself
                {
                    Vector3 directionToEnemy = closestEnemy.transform.position - transform.position;
                    directionToEnemy.y = 0f;
                    directionToEnemy = directionToEnemy.normalized;
                    transform.rotation = Quaternion.Slerp(transform.rotation.normalized, Quaternion.LookRotation(directionToEnemy), 50 * Time.deltaTime);
                    
                    float enemuDDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);
                        
                    if (enemuDDistance <= attackRange && !isAttacking)
                    {
                        anim.SetTrigger("attack");
                        lastAttackTime = 0;
                        isAttacking = true;
                    }
                    else if (enemuDDistance <= playerRange/2)
                    {
                        agent.SetDestination(closestEnemy.transform.position);
                    }
                        
                       
                }
                

            }

            if (attackCooldown <= lastAttackTime)
            {
                isAttacking = false;
            }
            lastAttackTime += Time.deltaTime;

            if (anim.GetFloat("speed") == 0f)
            {
                idleActionTimer += Time.deltaTime;

                if (idleActionTimer >= 10 && !isChilling)
                {
                    IdleAction();
                    isChilling = true;
                }
            }
            else
            {
                anim.SetTrigger("enemySpotted");
            }
            
        }
    }


    public void IdleAction()
    {
        anim.ResetTrigger("enemySpotted");
        anim.SetTrigger("chilling");
        idleActionTimer = 0f;
    }
    
    
    private Vector3 randomDestination;
    Vector3 GetRandomPointAroundPlayer()
    {
        Vector2 randomPoint = Random.insideUnitCircle.normalized * 2f;
        Vector3 randomDestination = player.transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
        return randomDestination;
    }

    
    public void StartDealDamage()
    {
        _damageDealer.StartDealDamage(0);
    }
    
    public void EndDealDamage()
    {
        _damageDealer.EndDealDamage();
    }
}