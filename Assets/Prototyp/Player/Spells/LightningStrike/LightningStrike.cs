using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    [SerializeField] GameObject[] enemy;
    List<GameObject> foundEnemies = new List<GameObject>(); // A list to keep track of found enemies
    private GameObject closestEnemy;

    [SerializeField] private GameObject lightningPrefab;
    
    [SerializeField] private float aggroRange; 

    [SerializeField] private float coolDownTime;
    private float timePassed;
    
    
    private void Start()
    {
        timePassed = 0f;
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
                }
            }
        }
    }

    private void Update()
    {
        if (coolDownTime <= timePassed)
        {
            FindAndAddEnemies();
            FindClosestEnemy();

            float distance = Vector3.Distance(this.transform.position, closestEnemy.transform.position);
            if (closestEnemy != null && distance <= aggroRange)
            {
                Instantiate(lightningPrefab, closestEnemy.transform);
                timePassed = 0f;
            }
        }

        timePassed += Time.deltaTime;
    }
}
