using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikePrefab : MonoBehaviour
{
    private Animation _animation;
    private GameObject player;

    [SerializeField]private float animationTime = 1f;
    private float timepassed;
    
    //dmg to the enemy
    List<GameObject> hasDealtDamage;
    private DamageOfEverything _damageOfEverything;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<HealthSystem>().gameObject;
        _damageOfEverything = player.GetComponent<DamageOfEverything>();
        hasDealtDamage = new List<GameObject>();
        
        _animation = gameObject.GetComponent<Animation>();
        _animation.Play();
        timepassed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        DealDamage();
        
        if (animationTime <= timepassed)
        {
            Destroy(this);
        }

        timepassed += Time.deltaTime;
    }

    [SerializeField] private float weaponLength;
    
    /*private void DealDamage()
    {
        RaycastHit hit;
 
        int layerMask = 1 << 9;
        if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
        {
            if (hit.transform.TryGetComponent(out Enemy enemy) && !hasDealtDamage.Contains(hit.transform.gameObject))
            {
                enemy.TakeDamage(_damageOfEverything.lightingStrikeDamage);
                enemy.HitVFX(hit.point);
                hasDealtDamage.Add(hit.transform.gameObject);
            }
        }
    }*/
    private Enemy enemy;
    private EnemyBoss enemyBoss;

    private void DealDamage()
    {
        if (gameObject.GetComponentInParent<Enemy>() != null)
        {
            enemy = gameObject.GetComponentInParent<Enemy>();
        }
        else if (gameObject.GetComponentInParent<EnemyBoss>() != null)
        {
            enemyBoss = gameObject.GetComponentInParent<EnemyBoss>();
        }
        else
        {
            return;
        }

        Transform parentTransform = gameObject.GetComponentInParent<Transform>();

        if (parentTransform != null)
        {
            Vector3 hitPoint = parentTransform.position; // Use the position of the parent

            if (!hasDealtDamage.Contains(parentTransform.gameObject))
            {
                if (enemy != null)
                {
                    float knockBackForce = 0f;
                    enemy.TakeDamage(_damageOfEverything.lightingStrikeDamage, parentTransform);
                    enemy.HitVFX(hitPoint);
                    hasDealtDamage.Add(parentTransform.gameObject);
                }
                else if (enemyBoss != null)
                {
                    float knockBackForce = 0f;
                    enemyBoss.TakeDamage(_damageOfEverything.lightingStrikeDamage, parentTransform);
                    enemyBoss.HitVFX(hitPoint);
                    hasDealtDamage.Add(parentTransform.gameObject);
                }
                
            }
        }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
