using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OckoProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10.0f;
    
    public float desiredHeight = 5.0f;
    public float desiredZPosition = 10.0f;

    public void FireProjectile(Vector3 targetPosition)
    {
        Vector3 forwardDirection = transform.forward;
        Vector3 spawnPosition = transform.position + forwardDirection * desiredZPosition;
        spawnPosition.y = desiredHeight +transform.position.y;

// Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

// Set the projectile's parent to the current GameObject (this)
        projectile.transform.parent = transform;
        ProjectileDamage projectileDamage = gameObject.GetComponentInChildren<ProjectileDamage>();
        projectileDamage.GetParent();
        projectile.transform.parent = null;


// Calculate the direction to the target
        Vector3 direction = (targetPosition - transform.position).normalized;

// Get the Rigidbody component and apply a force to the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = direction * projectileSpeed;
    }
    
    
}
