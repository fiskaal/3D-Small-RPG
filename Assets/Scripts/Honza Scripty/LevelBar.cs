using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    public Slider levelSlider; // Reference to the UI Slider representing the level bar
    public LevelSystem levelSystem; // Reference to the LevelSystem script

    public float sliderSpeed = 25f; // Adjust the speed of the slider movement

    void Start()
    {
        // Ensure the levelSlider and levelSystem references are not null
        if (levelSlider != null && levelSystem != null)
        {
            // Set the slider value based on the relative progress from currentExperience to experienceThreshold
            float progress = levelSystem.currentExperience / levelSystem.experienceThreshold;
            levelSlider.value = progress;
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
            // Update the slider value smoothly based on the relative progress from currentExperience to experienceThreshold
            float progress = levelSystem.currentExperience / levelSystem.experienceThreshold;
            levelSlider.value = Mathf.Lerp(levelSlider.value, progress, Time.deltaTime * sliderSpeed);

            // Optionally, you can update the max value of the slider if the experience threshold changes dynamically.
            // levelSlider.maxValue = 1f; // The slider max value is always 1 for representing progress
        }
    }
}
