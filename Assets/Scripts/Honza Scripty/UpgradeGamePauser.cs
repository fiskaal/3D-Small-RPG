using UnityEngine;

public class UpgradeGamePauser : MonoBehaviour
{
    void Update()
    {
        // Check if the object has at least one active child
        bool hasActiveChild = false;

        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                hasActiveChild = true;
                break;
            }
        }

        // Pause the game if there is at least one active child, otherwise continue the game
        if (hasActiveChild)
        {
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Continue the game
        }
    }
}
