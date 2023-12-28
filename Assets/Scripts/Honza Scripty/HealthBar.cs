using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; // Reference to the UI Slider representing the health bar
    public HealthSystem healthSystem; // Reference to the PlayerHP script

    public float sliderSpeed = 25f; // Adjust the speed of the slider movement

    void Update()
    {
        // Ensure the healthSlider and playerHP references are not null
        if (healthSlider != null && healthSystem != null)
        {
            // Update the slider value smoothly based on the player's health
            float targetSliderValue = MapHealthToSliderValue(healthSystem.health);
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetSliderValue, Time.deltaTime * sliderSpeed);
        }
    }

    // Map player's health to slider value (assuming health ranges from 0 to 100)
    float MapHealthToSliderValue(float health)
    {
        return Mathf.Clamp01(health / 100f);
    }
}
