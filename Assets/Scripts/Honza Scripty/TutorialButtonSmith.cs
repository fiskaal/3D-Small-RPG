using UnityEngine;
using UnityEngine.UI;

public class TutorialButtonSmith : MonoBehaviour
{
    // Reference to the button UI component
    public Button toggleButton;

    // Variable to set the initial state through the Unity Inspector
    public bool isSmithCompleteInitialState = true;

    private void Start()
    {
        // Add a listener to the button to respond to clicks
        toggleButton.onClick.AddListener(ToggleSmithComplete);
    }

    // Toggle the state of isSmithComplete and update the PlayerPrefs
    private void ToggleSmithComplete()
    {
        // Toggle the static variable directly
        TutorialManager.isSmithComplete = !TutorialManager.isSmithComplete;

        // Save the updated state to PlayerPrefs using the static method
        TutorialManager.SavePlayerPrefs();

        // Log the current state for testing purposes
        Debug.Log("isSmithComplete: " + TutorialManager.isSmithComplete);
    }
}
