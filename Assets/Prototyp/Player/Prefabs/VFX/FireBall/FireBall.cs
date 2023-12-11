using System;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float damage = 20f;
    public GameObject DestroyVFX;

    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(damage, transform);
                Instantiate(DestroyVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else if (other.TryGetComponent<EnemyBoss>(out EnemyBoss enemyBoss))
            {
                enemyBoss.TakeDamage(damage, transform);
                Instantiate(DestroyVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Player"))
        {
            
        }
        else if (other.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
        {
            
        }
        else
        {
            Instantiate(DestroyVFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        */
        
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(damage, transform);
                Instantiate(DestroyVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else if (other.TryGetComponent<EnemyBoss>(out EnemyBoss enemyBoss))
            {
                enemyBoss.TakeDamage(damage, transform);
                Instantiate(DestroyVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        
        
        if (!other.CompareTag("Player"))
        {
            if (other.gameObject != gameObject)
            {
                if (other.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
                {
                    Instantiate(DestroyVFX, transform.position, transform.rotation);
                    Destroy(gameObject);
                }
            }
        }
        
    }
}