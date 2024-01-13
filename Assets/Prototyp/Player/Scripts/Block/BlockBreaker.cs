using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlockBreaker : MonoBehaviour
{
    [Header("gameplay")] 
    public bool blocking;
    public int maxNumberOfBlocks = 3;
    public int upgradedMaxNumberOfBlocks = 5;
    public int blockableAttacks = 0; // Total number of blockable attacks
    public Character character;
    public GameObject blockBrokenVFX;
    public DamagePopUpGenerator popUpGenerator;

    private bool numberOfBlockIncreased = false;
    
    [SerializeField] public float blockRegenerationRate = 1.0f; // Time for regeneration in seconds
    public float blockRegenerationTimer = 0.0f; // Timer for block regeneration

    [SerializeField] public float blockBreakCooldownTime = 1.0f; // Time for block break cooldown in seconds
    public float blockBreakTimer = 0.0f;


    public PlayerAudioScript playerAudioScript;
    private void Start()
    {
        popUpGenerator = FindObjectOfType<DamagePopUpGenerator>();
    }

    private void Update()
    {
        if (!blocking)
        {
            // Regenerate blockable attacks over time
            blockRegenerationTimer += Time.deltaTime;
            if (blockRegenerationTimer >= blockRegenerationRate)
            {
                blockRegenerationTimer = 0.0f;
                if (blockableAttacks < maxNumberOfBlocks)
                {
                    blockableAttacks++;
                }
            }
        }

        // Cooldown for block break after it occurs
        if (character.blockBroken)
        {
            blockBreakTimer += Time.deltaTime;
            if (blockBreakTimer >= blockBreakCooldownTime)
            {
                character.blockBroken = false;
                Debug.Log("Block is ready!");
                blockBreakTimer = 0f;
            }
        }
        
        //Upgrade number of blocks with the block upgrade
        if (character.blockIsUgraded && !numberOfBlockIncreased)
        {
            maxNumberOfBlocks = upgradedMaxNumberOfBlocks;
            numberOfBlockIncreased = true;
        }
    }

    public void BlockAttackCounter()
    {
        if (blocking)
        {
            blockableAttacks--;
            // Check if the consecutive blocked attacks exceed the threshold
            if (blockableAttacks < 1)
            {
                BlockBreak();
            }
        }
    }

    private void BlockBreak()
    {
        popUpGenerator.CreatePopUp(character.gameObject.transform.position, "Shield Broken", Color.red);
        Debug.Log("Block Break!");
        character.blockBroken = true;
        
        playerAudioScript.PlayBlockBreak();
    }
}