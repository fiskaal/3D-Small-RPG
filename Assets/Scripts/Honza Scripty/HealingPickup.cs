using UnityEngine;

public class HealingPickup : MonoBehaviour
{
    public float healingAmount = 50f; // The amount of health to heal when picked up

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();

            if (playerHealth != null)
            {
                // Increase player's health
                playerHealth.health += healingAmount;

                // Clamp the player's health to a maximum value if needed
                // playerHealth.health = Mathf.Clamp(playerHealth.health, 0f, maxHealth);

                // Destroy the healing pickup object after it's picked up
                Destroy(gameObject);
            }
        }
    }
}
