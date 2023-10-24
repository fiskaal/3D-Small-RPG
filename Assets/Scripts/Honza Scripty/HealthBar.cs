using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; // Reference to the UI Slider representing the health bar
    public PlayerHP playerHP; // Reference to the PlayerHP script

    void Update()
    {
        // Ensure the healthSlider and playerHP references are not null
        if (healthSlider != null && playerHP != null)
        {
            // Update the slider value based on the player's health
            healthSlider.value = MapHealthToSliderValue(playerHP.currentHealth);
        }
    }

    // Map player's health to slider value (assuming health ranges from 0 to 100)
    float MapHealthToSliderValue(float health)
    {
        return Mathf.Clamp01(health / 100f);
    }
}
