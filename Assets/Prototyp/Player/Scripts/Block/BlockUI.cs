using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockUI : MonoBehaviour
{
    public int blockIndex; // Assign a unique index to each block UI element
    public TextMeshProUGUI timerText; // Reference to the UI text element for the timer
    [SerializeField] private Image cooldownImage;
    
    [SerializeField] private GameObject[] Shields;
    [SerializeField] private TextMeshProUGUI BlockBrokenText;

    
    // Reference to the BlockBreaker script
    public BlockBreaker blockBreaker;

    private void Start()
    {
        UpdateNumberOfShieldImages();
    }

    private void Update()
    {
        UpdateShieldBreakUi();
    }

    private void UpdateBlockBreakTimerText(float remainingTime)
    {
        BlockBrokenText.text = "Block broken " + remainingTime.ToString("F0");
    }

    public void UpdateNumberOfShieldImages()
    {
        int totalShields = Shields.Length;

        for (int i = 0; i < totalShields; i++)
        {
            if (i < blockBreaker.blockableAttacks)
            {
                // Activate shields up to blockableAttacks
                Shields[i].SetActive(true);
            }
            else
            {
                // Deactivate shields beyond blockableAttacks
                Shields[i].SetActive(false);
            }
        }
    }

    public void UpdateShieldBreakUi()
    {
        if (blockBreaker.character.blockBroken)
        {
            //show text block broken + cooldown
            if (BlockBrokenText.gameObject.activeSelf != true)
            {
                BlockBrokenText.gameObject.SetActive(true);
            }
            
            //UpdateText
            float remainingTime = blockBreaker.blockBreakCooldownTime - blockBreaker.blockBreakTimer;
            UpdateBlockBreakTimerText(remainingTime);
        }
        else
        {
            // Turn off block broken text
            if (BlockBrokenText.gameObject.activeSelf != false)
            {
                BlockBrokenText.gameObject.SetActive(false);
            }
        }
    }
}