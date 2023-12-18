using System;
using System.Collections;
using UnityEngine;

public class OckoProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10.0f;
    public float desiredTargetHeight = 1f;
    public Transform projectileHolder;
    public Transform boneProjectileHolderCopy;
    private GameObject projectile;

    public Vector3 targetPosition;
    private Transform shooterTransform; // Changed from "transform" to avoid conflicts with Unity's "transform"

    private Enemy _enemy;
    private EnemyBoss _enemyBoss;

    private void Start()
    {
        if (GetComponent<Enemy>() != null)
        {
            _enemy = GetComponent<Enemy>();
        }
        else if (GetComponent<EnemyBoss>() != null)
        {
            _enemyBoss = GetComponent<EnemyBoss>();
        }

        projectileHolder.transform.localScale = projectilePrefab.transform.localScale;
    }

    private void Update()
    {
        if (boneProjectileHolderCopy != null)
        {
            projectileHolder.position = boneProjectileHolderCopy.position;
        }
    }

    public void SpawnProjectile()
    {
        if (_enemy != null)
        {
            targetPosition = _enemy.player.transform.position;
            shooterTransform = _enemy.transform;
        }
        else if (_enemyBoss != null)
        {
            targetPosition = _enemyBoss.player.transform.position;
            shooterTransform = _enemyBoss.transform;
        }
        
        projectile = Instantiate(projectilePrefab, projectileHolder.position, Quaternion.identity, projectileHolder);
        projectile.transform.localScale = projectilePrefab.transform.localScale;
    }

    public void FireProjectile()
    {
        if (projectile == null)
        {
            return;
        }

        var projectileDamage = projectile.GetComponent<ProjectileDamage>();
        if (projectileDamage != null)
        {
            projectileDamage.shooterTransform = shooterTransform;
        }

        projectile.transform.parent = null;
        projectile.transform.localScale = projectilePrefab.transform.localScale;

        
        Vector3 targetPosition1 = targetPosition - projectileHolder.position;

        // Increase the target's height
        targetPosition1.y += desiredTargetHeight;
        // Calculate the direction to the target without considering height difference
        Vector3 direction = targetPosition1.normalized;

        // Get the Rigidbody component and set the velocity directly for stability
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
    }
}
