using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlockBreaker : MonoBehaviour
{
    [Header("gameplay")] public bool blocking;
    public int maxNumberOfBlocks = 3;
    public int blockableAttacks = 0; // Total number of blockable attacks
    public Character character;

    [SerializeField] public float blockRegenerationRate = 1.0f; // Time for regeneration in seconds
    public float blockRegenerationTimer = 0.0f; // Timer for block regeneration

    [SerializeField] public float blockBreakCooldownTime = 1.0f; // Time for block break cooldown in seconds
    public float blockBreakTimer = 0.0f;

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
        Debug.Log("Block Break!");
        character.blockBroken = true;
    }
}