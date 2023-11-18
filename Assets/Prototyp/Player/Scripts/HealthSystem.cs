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

    [Header("Block")] 
    [SerializeField]private BlockVFX blockVFXScript;
    
    
    void Start()
    {
        animator = GetComponent<Animator>();
        
        //shield spell
        _playerShield = gameObject.GetComponentInChildren<PlayerShield>();

        _character = GetComponent<Character>();
    }


    [SerializeField] private float angleThreshold;
    public void TakeDamage(float damageAmount, Transform enemyTransform)
    {
        _playerShield = gameObject.GetComponentInChildren<PlayerShield>();
        Transform highestParentTransform = GetHighestParentTransform(enemyTransform);

        
        // Get the direction from the player to the enemy
        Vector3 directionToEnemy = (highestParentTransform.position - transform.position).normalized;

        // Get the player's forward direction
        Vector3 playerForward = transform.forward;

        // Calculate the dot product between the player's forward direction and the direction to the enemy
        float dotProduct = Vector3.Dot(directionToEnemy, playerForward);

        // Define the angle threshold for the front area (e.g., 150 degrees)
        //float angleThreshold = 150f;
        
        if (_playerShield != null)
        {
            _playerShield.TakeDamage(1);
        }
        else
        {
            if (_character.blockingStateActive)
            {
                if (dotProduct < Mathf.Cos(angleThreshold * Mathf.Deg2Rad))
                {
                    // Enemy is behind the player
                    health -= damageAmount;
                    animator.SetTrigger("damage");
                    //CameraShake.Instance.ShakeCamera(2f, 0.2f);
                }
                else
                {
                    // Enemy is in front of the player, no damage or shield hit
                    animator.SetTrigger("blockDamage");
                    blockVFXScript.Damage();
                }
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
    
    private Transform GetHighestParentTransform(Transform childTransform)
    {
        Transform parent = childTransform;
        while (parent.parent != null)
        {
            parent = parent.parent;
        }
        return parent;
    }

    [Header("Death PopUp")] 
    [SerializeField]private GameObject deathUIPopUp;
    [SerializeField] private GameObject playerDestroy;
    void Die()
    {
        deathUIPopUp.SetActive(true);
        Instantiate(ragdoll, transform.position, transform.rotation);
        Destroy(playerDestroy);
        FreezeGame();
    }
    public void HitVFX(Vector3 hitPosition)
    {
        if (_character.blockingStateActive)
        {
            
        }
        else
        {
            GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
            Destroy(hit, 3f);
        }
    }

    void FreezeGame()
    {
        // Freeze the game by setting timeScale to 0
        Time.timeScale = 0f;

        // Show the game cursor (it's hidden by default when playing in Unity Editor)
        Cursor.visible = true;
    }
    
    [SerializeField] private float visualizationDistance = 2f;

    private void OnDrawGizmos()
    {
        // Draw a line to represent the player's forward direction
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * visualizationDistance);

        // Calculate and draw two lines representing the front area
        float halfAngle = angleThreshold / 2f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfAngle, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfAngle, Vector3.up);

        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + leftRayDirection * visualizationDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightRayDirection * visualizationDistance);
    }
}
