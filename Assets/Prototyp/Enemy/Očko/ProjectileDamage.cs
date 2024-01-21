using System;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public GameObject HitVFX;
    public Transform shooterTransform;
    [SerializeField] private float damage = 1f;
    private GameObject player;

    private void Start()
    {
        shooterTransform = GetComponentInParent<OckoProjectile>().transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Detected with: " + other.gameObject.name);
        // Check if collided with the player
        if (other.CompareTag("Player"))
        {
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, shooterTransform, transform, false);
                playerHealth.HitVFX(transform.position);
            }

            // Spawn a hit visual effect and destroy the projectile
            InstantiateHitVFX();
            DestroyProjectile();
        }

        // Check if collided with an object that is not the projectile's parent
        if (other.gameObject != gameObject)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
            {
                if (other.gameObject.layer != LayerMask.NameToLayer("UICollider"))
                {
                    if (!other.CompareTag("Enemy"))
                    {
                        InstantiateHitVFX();
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    private void InstantiateHitVFX()
    {
        GameObject hit = Instantiate(HitVFX, transform.position, transform.rotation);
        hit.transform.SetParent(null);
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    
}