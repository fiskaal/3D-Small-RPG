
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OckoProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10.0f;
    public float desiredZPosition = 10.0f;
    public float desiredHeight = 5.0f;
    public float desiredTargetHeight = 1f;
    
    
    public void FireProjectile(Vector3 targetPosition, Transform shooterTransform)
    {
        // Calculate the spawn position of the projectile
        Vector3 forwardDirection = transform.forward;
        Vector3 spawnPosition = transform.position + forwardDirection * desiredZPosition;
        spawnPosition.y += desiredHeight;

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        projectile.transform.parent = null;

        // Increase the target's height
        targetPosition.y += desiredTargetHeight;
        // Calculate the direction to the target without considering height difference
        Vector3 direction = (targetPosition - spawnPosition).normalized;

        // Get the Rigidbody component and apply a force to the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = direction * projectileSpeed;

        ProjectileDamage projectileScript = projectile.GetComponent<ProjectileDamage>();
        projectileScript.shooterTransform = shooterTransform;
    }
}
