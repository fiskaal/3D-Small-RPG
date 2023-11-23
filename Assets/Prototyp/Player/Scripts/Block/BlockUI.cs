using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockUI : MonoBehaviour
{
    public int blockIndex; // Assign a unique index to each block UI element
    public TextMeshProUGUI timerText; // Reference to the UI text element for the timer
    [SerializeField] private Image cooldownImage;

    
    // Reference to the BlockBreaker script
    public BlockBreaker blockBreaker;

    private void Update()
    {
        if (!blockBreaker.character.blockBroken)
        {
           
            
            // Calculate the fill amount based on the remaining cooldown time
            float fillAmount = 0 + (blockBreaker.blockRegenerationRate / blockBreaker.blockRegenerationTimer);
            cooldownImage.fillAmount = fillAmount;

            float remainingTime = blockBreaker.blockableAttacks;
            UpdateTimerText(remainingTime);
            
        }
        else
        {
            // Calculate the fill amount based on the remaining cooldown time
            float fillAmount = 0 + (blockBreaker.blockBreakCooldownTime / blockBreaker.blockBreakTimer);
            cooldownImage.fillAmount = fillAmount;

            float remainingTime = blockBreaker.blockBreakCooldownTime - blockBreaker.blockBreakTimer;
            UpdateBlockBreakTimerText(remainingTime);
        }


    }

    private void UpdateTimerText(float remainingTime)
    {
        // Display the timer text and update the countdown
        timerText.text = remainingTime.ToString("F0") + "/" + blockBreaker.maxNumberOfBlocks; // Format the time display as desired
    }

    private void UpdateBlockBreakTimerText(float remainingTime)
    {
        timerText.text = remainingTime.ToString("F0");
    }
}