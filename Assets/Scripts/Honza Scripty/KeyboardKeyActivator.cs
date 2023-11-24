using UnityEngine;

public class KeyboardKeyActivator : MonoBehaviour
{
    public GameObject objectToActivate; // Reference to the GameObject you want to activate

    [SerializeField]
    private KeyCode activationKey = KeyCode.Y; // Default activation key is Y

    void Update()
    {
        // Check if the specified button is pressed
        if (Input.GetKeyDown(activationKey))
        {
            // Check if the GameObject reference is not null
            if (objectToActivate != null)
            {
                // Activate the specified GameObject
                objectToActivate.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Please assign a GameObject to be activated in the inspector.");
            }
        }
    }
}
