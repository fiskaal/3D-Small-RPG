using System;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float damage = 20f;
    public GameObject DestroyVFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(damage, transform);
                Destroy(gameObject);
            }
            else if (other.TryGetComponent<EnemyBoss>(out EnemyBoss enemyBoss))
            {
                enemyBoss.TakeDamage(damage, transform);
                Destroy(gameObject);
            }
        }
        
        if (other.gameObject != gameObject)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
            {
                if (!other.CompareTag("Player"))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    
    private void OnDestroy()
    {
        Instantiate(DestroyVFX, transform.position, transform.rotation);
    }
}