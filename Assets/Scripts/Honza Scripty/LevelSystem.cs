using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSystem : MonoBehaviour
{
    public int playerLevel = 1; // Current level of the player
    public float experienceThreshold = 100f; // Experience points required to level up
    public float currentExperience = 0f; // Current experience points of the player
    public GameObject upgradePanels; // Reference to the parent object containing UpgradePanels
    public Text levelText; // Reference to the Text component for displaying player level

    // List to store indices of activated child objects to avoid duplication
    List<int> activatedIndices = new List<int>();

    // Method to increase player's experience points
    public void GainExperience(float experiencePoints)
    {
        currentExperience += experiencePoints;

        // Check if the player has enough experience points to level up
        if (currentExperience >= experienceThreshold)
        {
            LevelUp();
        }
    }

    // Method to handle player level up
    void LevelUp()
    {
        playerLevel++; // Increase player level by 1
        currentExperience = 0f; // Reset current experience points
        experienceThreshold = CalculateExperienceThreshold(); // Calculate new experience threshold for the next level

        // Clear the list of activated indices for the next level up
        activatedIndices.Clear();

        // Activate random UpgradePanels for the next level
        ActivateRandomUpgradePanels();

        // Update the level text to display the new player level
        UpdateLevelText();

        // Perform any other actions related to level up (e.g., unlocking new abilities, updating UI, etc.)
        Debug.Log("Level Up! New Level: " + playerLevel);

        Time.timeScale = 0f;
    }

    // Method to calculate experience threshold for the next level (you can customize the formula)
    float CalculateExperienceThreshold()
    {
        // Example: Experience threshold doubles with each level
        return experienceThreshold * 2f; // Return a float value
    }

    // Method to activate 3 random child UpgradePanels
    void ActivateRandomUpgradePanels()
    {
        if (upgradePanels != null)
        {
            // Clear the list of activated indices at the beginning of the method
            activatedIndices.Clear();

            int childCount = upgradePanels.transform.childCount;

            // Activate 3 random child objects or activate all if there are less than 3
            int panelsToActivate = Mathf.Min(3, childCount);

            for (int i = 0; i < panelsToActivate; i++)
            {
                int randomChildIndex;
                GameObject randomChild;

                // Ensure the selected child is not already activated
                do
                {
                    randomChildIndex = Random.Range(0, childCount);
                } while (activatedIndices.Contains(randomChildIndex));

                // Mark the index as activated to avoid duplication
                activatedIndices.Add(randomChildIndex);

                // Activate the random child object
                randomChild = upgradePanels.transform.GetChild(randomChildIndex).gameObject;
                randomChild.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("UpgradePanels not assigned!");
        }
    }



    // Method to deactivate all child objects of upgradePanels
    void DeactivateAllUpgradePanels()
    {
        if (upgradePanels != null)
        {
            foreach (Transform child in upgradePanels.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    // Method to update the level text
    void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = playerLevel.ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Deactivate all child objects of upgradePanels at the start
        DeactivateAllUpgradePanels();

        // Update the level text at the start
        UpdateLevelText();
    }
}
