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

            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0f;
            directionToPlayer = directionToPlayer.normalized;
            
            if (distanceToPlayer > playerRange)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation.normalized, Quaternion.LookRotation(directionToPlayer), 20 * Time.deltaTime);
                randomDestination = GetRandomPointAroundPlayer();
                agent.SetDestination(randomDestination);
                isChilling = false;
            }
            else
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerRange, enemyLayer);

                foreach (var collider in hitColliders)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);

                    if (distanceToEnemy <= playerRange / 2)
                    {
                        if (distanceToEnemy < closestDistance)
                        {
                            closestDistance = distanceToEnemy;
                            closestEnemy = collider.gameObject;
                        }
                    }
                }


                if (closestEnemy.gameObject != gameObject && closestEnemy != null) // Ensure the pet doesn't attack itself
                {
                    
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