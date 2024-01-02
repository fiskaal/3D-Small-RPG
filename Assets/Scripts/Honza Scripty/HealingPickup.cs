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
                // Calculate the remaining health capacity
                float remainingHealthSpace = playerHealth.maxHealth - playerHealth.health;

                // Heal by the minimum between healingAmount and remainingHealthSpace
                float actualHealing = Mathf.Min(healingAmount, remainingHealthSpace);

                // Increase player's health
                playerHealth.health += actualHealing;

                // Destroy the healing pickup object after it's picked up
                Destroy(gameObject);
            }
        }
    }
}
