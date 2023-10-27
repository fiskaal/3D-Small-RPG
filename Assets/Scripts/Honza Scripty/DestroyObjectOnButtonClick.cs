using UnityEngine;
using UnityEngine.UI;

public class DestroyObjectOnButtonClick : MonoBehaviour
{
    // Reference to the GameObject you want to destroy
    public GameObject objectToDestroy;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Button component from the GameObject
        Button button = GetComponent<Button>();

        // Add a listener for the button click event
        button.onClick.AddListener(OnButtonClick);
    }

    // Method to handle button click
    void OnButtonClick()
    {
        // Check if the object to destroy exists
        if (objectToDestroy != null)
        {
            // Destroy the specified GameObject
            Destroy(objectToDestroy);

            // Optionally, you can perform other actions after destroying the object
            // For example, you can print a message to the console
            Debug.Log("Object Destroyed!");
        }
        else
        {
            // Print a message to the console if the object is null
            Debug.LogWarning("ObjectToDestroy is null. Assign a GameObject to be destroyed in the Inspector.");
        }
    }
}
