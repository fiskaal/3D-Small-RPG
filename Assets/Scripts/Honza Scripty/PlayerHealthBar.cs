using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider healthSlider; // Reference to the UI Slider representing the health bar
    public HealthSystem healthSystem; // Reference to the HealthSystem script

    public float sliderSpeed = 5f; // Adjust the speed of the slider movement

    void Update()
    {
        // Ensure the healthSlider and healthSystem references are not null
        if (healthSlider != null && healthSystem != null)
        {
            // Update the slider value smoothly based on the player's health and max health
            UpdateHealthSlider(healthSystem.health, healthSystem.maxHealth);
        }
    }

    // Update the health slider smoothly based on current health and max health
    void UpdateHealthSlider(float currentHealth, float maxHealth)
    {
        // Ensure maxHealth is greater than 0 to avoid division by zero
        if (maxHealth > 0)
        {
            // Calculate the normalized position of the current health within the max health range
            float normalizedHealthPosition = Mathf.Clamp01(currentHealth / maxHealth);

            // Smoothly interpolate between the current slider value and the target value
            float targetSliderValue = normalizedHealthPosition;
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetSliderValue, Time.deltaTime * sliderSpeed);
        }
        else
        {
            // Handle the case where maxHealth is 0 or less
            healthSlider.value = 0f;
        }
    }
}
