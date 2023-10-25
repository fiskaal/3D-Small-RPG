using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    bool canDealDamage;
    List<GameObject> hasDealtDamage;

    [SerializeField] float weaponLength;
    [SerializeField] float weaponDamage;
    
    [SerializeField] GameObject[] enemy;
    List<GameObject> foundEnemies = new List<GameObject>(); // A list to keep track of found enemies
    [SerializeField] float aggroRange = 4f;
    private GameObject closestEnemy;
    private bool attackedEnemySelected;

    private GameObject player;
    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();
        
        player = GameObject.FindGameObjectWithTag("Player");
        closestEnemy = null;
        attackedEnemySelected = false;
        
        FindAndAddEnemies();
        
    }
    
    void FindAndAddEnemies()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (!foundEnemies.Contains(allEnemies[i]))
            {
                // Add the enemy to the array and to the list of found enemies
                for (int j = 0; j < enemy.Length; j++)
                {
                    if (enemy[j] == null)
                    {
                        enemy[j] = allEnemies[i];
                        foundEnemies.Add(allEnemies[i]);
                        break; // Break out of the inner loop
                    }
                }
            }
        }
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
                    enemy.TakeDamage(weaponDamage);
                    enemy.HitVFX(hit.point);
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
            }

            if (!attackedEnemySelected)
            {
                FindClosestEnemy();
            }

            LookAtEnemy();
        }
    }
    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealtDamage.Clear();
    }

    public void FindClosestEnemy()
    {
        float closestDistance = float.MaxValue;
        for (int i = 0; i < foundEnemies.Count; i++)
        {
            if (foundEnemies[i] == null)
            {
                // Remove destroyed enemy from the list
                foundEnemies.RemoveAt(i);
                i--; // Adjust the loop counter
            }
            else
            {
                float distance = Vector3.Distance(foundEnemies[i].transform.position, transform.position);
                if (distance <= aggroRange && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = foundEnemies[i];
                    attackedEnemySelected = true;
                }
            }
        }
    }

    public GameObject attackedEnemyPopUp;
    public void LookAtEnemy()
    {
        if (closestEnemy != null)
        {
            player.transform.LookAt(closestEnemy.transform);
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
