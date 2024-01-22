using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnCollision : MonoBehaviour
{
    // The name of the scene you want to load
    public string sceneName = "TUTORIAL_level";

    // The spawn coordinates for the player
    public Vector3 spawnCoordinates;

    // This function is called when the Collider other enters the trigger
    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Set the player's position to the spawn coordinates
            other.transform.position = spawnCoordinates;

            // Load the specified scene
            SceneManager.LoadScene(sceneName);
        }
    }
}
