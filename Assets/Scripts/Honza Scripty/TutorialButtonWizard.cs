using UnityEngine;
using UnityEngine.UI;

public class TutorialButtonWizard : MonoBehaviour
{
    // Reference to the button UI component
    public Button toggleButton;

    // Variable to set the initial state through the Unity Inspector
    public bool isWizardCompleteInitialState = true;

    private void Start()
    {

        // Add a listener to the button to respond to clicks
        toggleButton.onClick.AddListener(ToggleWizardComplete);
    }

    // Toggle the state of isWizardComplete and update the PlayerPrefs
    private void ToggleWizardComplete()
    {
        // Toggle the static variable directly
        TutorialManager.isWizardComplete = !TutorialManager.isWizardComplete;

        // Save the updated state to PlayerPrefs using the static method
        TutorialManager.SavePlayerPrefs();

        // Log the current state for testing purposes
        Debug.Log("isWizardComplete: " + TutorialManager.isWizardComplete);
    }
}
