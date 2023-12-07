/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerFireProjectile : MonoBehaviour
{
    public float projectileSpeed = 1;
    public GameObject projectilePrefab;
    public GameObject projectileHolder;
    public EnemyTarget enemyTarget;


    private GameObject currentProjectile;

    public float desiredTargetHeight = 1;


    public void SpawnProjectile()
    {
        currentProjectile = Instantiate(projectilePrefab, projectileHolder.transform);
    }

    public void FireProjectile()
    {
        currentProjectile.transform.parent = null;

        Vector3 targetPosition = enemyTarget.currentTarget.transform.position;

        if (enemyTarget.currentTarget == null)
        {
            Destroy(currentProjectile);
            return;
        }
        // Increase the target's height
        targetPosition.y += desiredTargetHeight;
        // Calculate the direction to the target without considering height difference
        Vector3 direction = targetPosition.normalized;

        // Get the Rigidbody component and apply a force to the projectile
        Rigidbody rb = currentProjectile.GetComponent<Rigidbody>();
        rb.velocity = direction * projectileSpeed;
    }
}
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireProjectile : MonoBehaviour
{
    public float projectileSpeed = 1;
    public GameObject projectilePrefab;
    public GameObject projectileHolder;
    public EnemyTarget enemyTarget;

    private GameObject currentProjectile;

    public float desiredTargetHeight = 1;

    public void SpawnProjectile()
    {
        currentProjectile = Instantiate(projectilePrefab, projectileHolder.transform);
    }

    public void FireProjectile()
    {
        currentProjectile.transform.parent = null;

        if (enemyTarget.currentTarget == null)
        {
            // If there's no current target, shoot the projectile forward
            ShootForward();
            return;
        }

        Vector3 targetPosition = enemyTarget.currentTarget.transform.position - projectileHolder.transform.position;

        // Increase the target's height
        targetPosition.y += desiredTargetHeight;
        // Calculate the direction to the target without considering height difference
        Vector3 direction = targetPosition.normalized;

        // Get the Rigidbody component and set the velocity directly for stability
        Rigidbody rb = currentProjectile.GetComponent<Rigidbody>();
        rb.velocity = direction * projectileSpeed;
    }

    private void ShootForward()
    {
        // Calculate forward direction based on the current rotation
        Vector3 forwardDirection = enemyTarget.cameraTransform.forward;

        Rigidbody rb = currentProjectile.GetComponent<Rigidbody>();
        rb.velocity = forwardDirection * projectileSpeed;
    }
}
