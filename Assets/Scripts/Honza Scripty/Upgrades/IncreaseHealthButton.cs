using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseHealthButton : MonoBehaviour
{
    public HealthSystem playerHealthScript; // Reference to the PlayerHP script attached to the player GameObject.
    public float increaseAmount = 10f; // The amount by which health will be increased.

    void Start()
    {
        // Get the PlayerHP script attached to the player GameObject.
        playerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();

        // Find the Button component on the GameObject this script is attached to and attach a method to its click event.
        Button increaseHealthButton = GetComponent<Button>();
        increaseHealthButton.onClick.AddListener(IncreaseHealth);
    }

    // Method to increase both maxHealth and currentHealth by the specified amount.
    void IncreaseHealth()
    {
        // Increase health
        playerHealthScript.health += increaseAmount;
    }
}
