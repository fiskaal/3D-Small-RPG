using UnityEngine;

public class HealingZone : MonoBehaviour
{
    [SerializeField] private float healAmount = 10f; // Adjust the healing amount as needed
    [SerializeField] private float healInterval = 1f; // Adjust the healing interval as needed

    private HealthSystem healthSystem;
    private float timeSinceLastHeal;

    private void Start()
    {
        healthSystem = FindObjectOfType<HealthSystem>(); // You may want to find the HealthSystem differently based on your project structure
    }

    private void Update()
    {
        if (healthSystem != null)
        {
            timeSinceLastHeal += Time.deltaTime;

            // Check if enough time has passed since the last heal
            if (timeSinceLastHeal >= healInterval)
            {
                // Instantly set the player's health to the maximum value
                healthSystem.health = Mathf.Min(healthSystem.health + healAmount, healthSystem.maxHealth);

                // Reset the timer
                timeSinceLastHeal = 0f;
            }
        }
    }
}
