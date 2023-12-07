using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float damage = 20f;
    public GameObject DestroyVFX;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //return;
        }

        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<Enemy>())
            {
                other.GetComponent<Enemy>().TakeDamage(damage, transform);
            }
            else if (other.GetComponent<EnemyBoss>())
            {
                other.GetComponent<EnemyBoss>().TakeDamage(damage, transform);
            }
        }

        GameObject vfx = Instantiate(DestroyVFX, transform);
        vfx.transform.SetParent(null);
        Destroy(gameObject);
    }
}
