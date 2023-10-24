using UnityEngine;

public class ExperiencePickup : MonoBehaviour
{
    public float experienceAmount = 10f; // Amount of experience points the pickup gives to the player

    void OnTriggerEnter(Collider other)
    {
        // Check if the pickup collides with the player (assuming the player has a collider set as trigger)
        if (other.CompareTag("Player"))
        {
            // Get the LevelSystem component from the player GameObject
            LevelSystem levelSystem = other.GetComponent<LevelSystem>();

            // Check if the LevelSystem component is found on the player
            if (levelSystem != null)
            {
                // Increase the player's currentExperience by the experienceAmount
                levelSystem.GainExperience(experienceAmount);

                // Optionally, you can play a pickup sound, deactivate the pickup object, or perform other actions.

                // Destroy the pickup object after it has been collected
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("LevelSystem component not found on the player!");
            }
        }
    }
}
