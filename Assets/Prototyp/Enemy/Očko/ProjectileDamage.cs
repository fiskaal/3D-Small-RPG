using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    private GameObject parent;

    public GameObject HitVFX;

    public Transform shooterTransform;

    [SerializeField] private float damage = 1f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();
            playerHealth.TakeDamage(damage, shooterTransform, transform);
            playerHealth.HitVFX(transform.position);
            GameObject hit = Instantiate(HitVFX, transform);
            hit.transform.SetParent(null);
            Destroy(gameObject);
        }
        
        
        if (other.gameObject != parent)
        {
            if (other.gameObject != gameObject)
            {
                GameObject hit = Instantiate(HitVFX, transform);
                hit.transform.SetParent(null);
                // Destroy the projectile when colliding with an object that is not its parent
                Destroy(gameObject);
            }
        }
    }

    public void GetParent()
    {
        parent = transform.parent.gameObject;
    }

    public void OnDestroy()
    {
         
    }
}
