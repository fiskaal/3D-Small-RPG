using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    bool canDealDamage;
    List<GameObject> hasDealtDamage;

    [SerializeField] float weaponLength;
    [SerializeField] float weaponDamage;
    private float finalDamage;
    
    private GameObject closestEnemy;
    private bool attackedEnemySelected;

    private GameObject player;
    private DamageOfEverything _damageOfEverything;

    private bool enemyTargetted;
    
    [SerializeField] private EnemyTarget _enemyTarget;
    
    void Start()
    {
        //get weapon damage
        _damageOfEverything = gameObject.GetComponentInParent<DamageOfEverything>();
        
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();

        
        player = FindObjectOfType<HealthSystem>().gameObject;
        closestEnemy = null;
        attackedEnemySelected = false;

        enemyTargetted = false;
        _enemyTarget = FindObjectOfType<EnemyTarget>();
    }
    
    void Update()
    {
        
        if (canDealDamage)
        {
            RaycastHit hit;
 
            int layerMask = 1 << 9;
            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                if (hit.transform.TryGetComponent(out Enemy enemy) && !hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    enemy.TakeDamage(finalDamage, hit.transform);
                    enemy.HitVFX(hit.point);
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
                
                if (hit.transform.TryGetComponent(out EnemyBoss enemyBoss) && !hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    enemyBoss.TakeDamage(finalDamage, hit.transform);
                    enemyBoss.HitVFX(hit.point);
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
            }

            if (!attackedEnemySelected && !enemyTargetted)
            {
                LookAtEnemy();
            }
        }
    }
    
    public void StartDealDamage(int damageAmplification)
    {
        if (damageAmplification != 0)
        {
            finalDamage = _damageOfEverything.weaponDamage;
            finalDamage = damageAmplification * finalDamage;
        }
        else if (_damageOfEverything != null)
        {
            finalDamage = _damageOfEverything.weaponDamage;
        }
        else
        {
            finalDamage = weaponDamage;
        }
        
        
        enemyTargetted = false;
        canDealDamage = true;
        hasDealtDamage.Clear();
        
    }
   
    public void LookAtEnemy()
    {
        if (_enemyTarget.currentTarget != null)
        {
            closestEnemy = _enemyTarget.currentTarget;
            Vector3 target = closestEnemy.transform.position - player.transform.position;
            target.y = 0f;

            if (closestEnemy != null)
            {
                //player.transform.LookAt(closestEnemy.transform);

                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(target),
                    200 * Time.deltaTime);
                enemyTargetted = true;
            }
        }
    }
    public void EndDealDamage()
    {
        canDealDamage = false;

        attackedEnemySelected = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
