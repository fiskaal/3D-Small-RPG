using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;


public class HealthSystem : MonoBehaviour
{
    [SerializeField] private BlockBreaker _blockBreaker;
    [SerializeField] public float maxHealth = 100f; // Maximum health value
    public float health = 100;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject deathVFX;

    Animator animator;

    //shield spell
    private PlayerShield _playerShield;
    private Character _character;

    //damage popup
    [SerializeField] private DamagePopUpGenerator _damagePopUpGenerator;

    [Header("Block")]
    [SerializeField] private BlockVFX blockVFXScript;

    [Header("Damage Quest")]
    private LvlQuestManager _lvlQuestManager;

    // Array of ArmorItem instances
    [SerializeField] private ArmorItem[] armorItems;

    // Previous total armor bonus
    private float previousTotalArmorBonus = 0f;

    public PlayerAudioScript audioScript;

    void Start()
    {
        _lvlQuestManager = FindObjectOfType<LvlQuestManager>();
        animator = GetComponent<Animator>();

        //shield spell
        _playerShield = gameObject.GetComponentInChildren<PlayerShield>();
        _character = GetComponent<Character>();

        // Apply armor bonus to max health
        UpdateHealthWithArmor();

        // Set the initial health to be equal to maxHealth
        health = maxHealth;
    }

    void Update()
    {
        // Check for changes in equipped armor and update health accordingly
        UpdateHealthWithArmor();
    }

    private void UpdateHealthWithArmor()
    {
        float totalArmorBonus = 0f;

        // Calculate the total armor bonus from active armor items
        foreach (var armorItem in armorItems)
        {
            // Check if the armor item is active before applying its bonus
            if (armorItem.gameObject.activeSelf)
            {
                totalArmorBonus += armorItem.GetArmorBonus();
            }
        }

        // Deduct the previous total armor bonus from maxHealth
        maxHealth -= previousTotalArmorBonus;

        // Update maxHealth with the new total armor bonus
        maxHealth += totalArmorBonus;

        // Ensure that health is within the valid range [0, maxHealth]
        health = Mathf.Clamp(health, 0f, maxHealth);

        // Update the previous total armor bonus for the next frame
        previousTotalArmorBonus = totalArmorBonus;

    }


    [SerializeField] private float angleThreshold;
    public void TakeDamage(float damageAmount, Transform enemyTransform, Transform hit, bool heavyDamage)
    {
        _playerShield = gameObject.GetComponentInChildren<PlayerShield>();
        Transform highestParentTransform = GetHighestParentTransform(enemyTransform);

        // Get the direction from the player to the enemy
        Vector3 directionToEnemy = (highestParentTransform.position - transform.position).normalized;

        // Get the player's forward direction
        Vector3 playerForward = transform.forward;

        // Calculate the dot product between the player's forward direction and the direction to the enemy
        float dotProduct = Vector3.Dot(directionToEnemy, playerForward);

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
                    if (heavyDamage != true)
                    {
                        animator.SetTrigger("damage");
                        _damagePopUpGenerator.CreatePopUp(hit.position, damageAmount.ToString(), Color.red);
                        audioScript.PlayHit();
                    }
                    else
                    {
                        animator.applyRootMotion = true;
                        animator.SetTrigger("heavyDamage");
                        _damagePopUpGenerator.CreatePopUp(hit.position, damageAmount.ToString(), Color.red);
                        audioScript.PlayHit();
                    }

                    //quest
                    if (_lvlQuestManager != null)
                    {
                        _lvlQuestManager.UpdateDamageQuest(damageAmount);
                    }
                }
                else
                {
                    // Enemy is in front of the player, no damage or shield hit
                    animator.SetTrigger("blockDamage");
                    blockVFXScript.Damage();
                    _blockBreaker.BlockAttackCounter();
                    _damagePopUpGenerator.CreatePopUp(hit.position, "Blocked", Color.cyan);
                    audioScript.PlayBlock();
                }
            }
            else
            {
                health -= damageAmount;
                if (heavyDamage != true)
                {
                    animator.SetTrigger("damage");
                    _damagePopUpGenerator.CreatePopUp(hit.position, damageAmount.ToString(), Color.red);
                    audioScript.PlayHit();
                }
                else
                {
                    animator.applyRootMotion = true;
                    animator.SetTrigger("heavyDamage");
                    _damagePopUpGenerator.CreatePopUp(hit.position, damageAmount.ToString(), Color.red);
                    audioScript.PlayHit();
                }

                //quest
                if (_lvlQuestManager != null)
                {
                    _lvlQuestManager.UpdateDamageQuest(damageAmount);
                }
            }
        }
        
        if (health <= 0)
        {
            Die();
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
    [SerializeField] private GameObject deathUIPopUp;
    [SerializeField] private GameObject playerDestroy;
    [SerializeField] private Transform deathVFXHolder;

    void Die()
    {
        Character character = GetComponent<Character>();
        character.enabled = false;
        //deathUIPopUp.SetActive(true);
        animator.SetBool("dead", true);
        animator.SetTrigger("death");
        //Instantiate(ragdoll, transform.position, transform.rotation);
        //Destroy(playerDestroy);
        StartCoroutine(FreezeGameAfterDelay(5f));
        Instantiate(deathVFX, deathVFXHolder);
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

    IEnumerator FreezeGameAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        // Freeze the game by setting timeScale to 0
        Time.timeScale = 0f;

        deathUIPopUp.SetActive(true);
        // Show the game cursor (it's hidden by default when playing in Unity Editor)
        Cursor.visible = true;
    }

    public void HeavyDamageIn()
    {
        _character.enabled = false;
    }

    public void HeavyDamageOut()
    {
        _character.enabled = true;
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
