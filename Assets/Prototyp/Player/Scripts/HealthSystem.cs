using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] public float health = 100;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;

    Animator animator;
    
    //shield spell
    private PlayerShield _playerShield;
    private Character _character;
    
    
    void Start()
    {
        animator = GetComponent<Animator>();
        
        //shield spell
        _playerShield = gameObject.GetComponentInChildren<PlayerShield>();

        _character = GetComponent<Character>();
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
            if (_character.blockingStateActive)
            {
                animator.SetTrigger("blockDamage");
            }
            else
            {
                health -= damageAmount;
                animator.SetTrigger("damage");
                //CameraShake.Instance.ShakeCamera(2f, 0.2f);
            }


            if (health <= 0)
            {
                Die();
            } 
        }
    }

    [Header("Death PopUp")] 
    [SerializeField]private GameObject deathUIPopUp;
    [SerializeField] private GameObject playerDestroy;
    void Die()
    {
        deathUIPopUp.SetActive(true);
        Instantiate(ragdoll, transform.position, transform.rotation);
        Destroy(playerDestroy);
    }
    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);

    }
}
