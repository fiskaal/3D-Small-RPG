using UnityEngine;
using UnityEngine.UI;

public class ContinueGame : MonoBehaviour
{
    private Button button; // Reference to the button component

    private void Start()
    {
        // Get the Button component attached to the GameObject
        button = GetComponent<Button>();

        // Add a listener to the button click event
        if (button != null)
        {
            button.onClick.AddListener(SetTimeScaleToOne);
        }
        else
        {
            Debug.LogError("Button component not found on GameObject: " + gameObject.name);
        }
    }

    // This method will be called when the button is clicked
    private void SetTimeScaleToOne()
    {
        // Set the time scale to 1
        Time.timeScale = 1f;

        // Optional: You can print a message to the console to verify that the button click is working
        Debug.Log("Time scale set to 1");
    }
}
