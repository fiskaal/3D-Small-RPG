using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    public Slider levelSlider; // Reference to the UI Slider representing the level bar
    public LevelSystem levelSystem; // Reference to the LevelSystem script

    void Start()
    {
        // Ensure the levelSlider and levelSystem references are not null
        if (levelSlider != null && levelSystem != null)
        {
            // Set the slider max value to the initial experience threshold of the LevelSystem
            levelSlider.maxValue = levelSystem.experienceThreshold;

            // Update the slider value based on the player's current experience points
            levelSlider.value = levelSystem.currentExperience;
        }
        else
        {
            Debug.LogError("LevelSlider or LevelSystem references not set!");
        }
    }

    void Update()
    {
        // Ensure the levelSlider and levelSystem references are not null
        if (levelSlider != null && levelSystem != null)
        {
            // Update the slider value based on the player's current experience points
            levelSlider.value = levelSystem.currentExperience;

            // Optionally, you can update the max value of the slider if the experience threshold changes dynamically.
            // levelSlider.maxValue = levelSystem.experienceThreshold;
        }
    }
}
