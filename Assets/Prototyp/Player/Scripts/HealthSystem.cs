using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;

    Animator animator;
    
    //shield spell
    private PlayerShield _playerShield;
    
    
    void Start()
    {
        animator = GetComponent<Animator>();
        
        //shield spell
        _playerShield = gameObject.GetComponentInChildren<PlayerShield>();
    }

   

    public void TakeDamage(float damageAmount)
    {
        _playerShield = gameObject.GetComponentInChildren<PlayerShield>();
        
        if (_playerShield != null)
        {
            _playerShield.TakeDamage(1);
        }
        else
        {
            health -= damageAmount;
            animator.SetTrigger("damage");
            //CameraShake.Instance.ShakeCamera(2f, 0.2f);


            if (health <= 0)
            {
                Die();
            } 
        }
    }

    void Die()
    {
        Instantiate(ragdoll, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);

    }
}
