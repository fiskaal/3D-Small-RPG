using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 3;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;

    [Header("Combat")]
    [SerializeField] float attackCD = 3f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float aggroRange = 4f;

    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    float timePassed;
    float newDestinationCD = 0.5f;

    private bool dead;
    private float timePassedAfterDeath;

    private bool firstTimeSpotted;
    private float screamTimePassed = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        animator.applyRootMotion = false;
        dead = false;
        timePassedAfterDeath = 0f;
        firstTimeSpotted = true;
        timePassed = attackCD;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);

		if (player == null)
		{
            return;
		}

        if (timePassed >= attackCD)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
            {
                animator.applyRootMotion = true;
                animator.SetTrigger("attack");
                timePassed = 0;
            }
        }
        timePassed += Time.deltaTime;

        if (newDestinationCD <= 0 && Vector3.Distance(player.transform.position, transform.position) <= aggroRange && !dead)
        {
            float screamTime = 1f;
            if (firstTimeSpotted)
            {
                animator.SetTrigger("enemySpotted");

                if (screamTimePassed >= screamTime)
                {
                    firstTimeSpotted = false;
                }

                screamTimePassed += Time.deltaTime;
                return;
            }

            if (!dead)
            {

                transform.LookAt(player.transform);
            }

            if (!firstTimeSpotted)
            {
                newDestinationCD = 0.5f;
                agent.SetDestination(player.transform.position);
            }
        }
        newDestinationCD -= Time.deltaTime;

        //if (!dead)
        {
            
            //transform.LookAt(player.transform);
        }

        //waiting for death animation to be over
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
            print(true);
            player = collision.gameObject;
        }
    }

	void Die()
    {
        //Instantiate(ragdoll, transform.position,transform.rotation);
        Destroy(this.gameObject);
    }

    public void TakeDamage(float damageAmount)
    {
        if (!dead)
        {
            health -= damageAmount;
            animator.applyRootMotion = true;
            animator.SetTrigger("damage");
            //CameraShake.Instance.ShakeCamera(2f, 0.2f);

            if (health <= 0)
            {
                animator.SetTrigger("death");
                dead = true;
                //Die();
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}
