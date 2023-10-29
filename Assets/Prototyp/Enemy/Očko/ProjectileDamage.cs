using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    private GameObject parent;

    [SerializeField] private float damage = 1f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();
            playerHealth.TakeDamage(damage);
            playerHealth.HitVFX(transform.position);
        }
        
        
        if (other.gameObject != parent)
        {
            // Destroy the projectile when colliding with an object that is not its parent
            Destroy(gameObject);
        }
    }

    public void GetParent()
    {
        parent = transform.parent.gameObject;
    }
}
