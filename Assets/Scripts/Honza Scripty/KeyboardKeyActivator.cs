using UnityEngine;

public class KeyboardKeyActivator : MonoBehaviour
{
    public GameObject objectToActivate; // Reference to the GameObject you want to activate

    [SerializeField]
    private KeyCode activationKey = KeyCode.Y; // Default activation key is Y

    private bool gameObjectIsAcite = false;

    void Update()
    {
        // Check if the specified button is pressed
        if (Input.GetKeyDown(activationKey))
        {
            if (objectToActivate.activeSelf != true)
            {
                // Check if the GameObject reference is not null
                if (objectToActivate != null)
                {
                    // Activate the specified GameObject
                    objectToActivate.SetActive(true);
                    gameObjectIsAcite = true;
                }
                else
                {
                    Debug.LogWarning("Please assign a GameObject to be activated in the inspector.");
                }
            }
            else
            {
                // Check if the GameObject reference is not null
                if (objectToActivate != null)
                {
                    // Activate the specified GameObject
                    objectToActivate.SetActive(false);
                    gameObjectIsAcite = false;
                }
                else
                {
                    Debug.LogWarning("Please assign a GameObject to be activated in the inspector.");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (objectToActivate.activeSelf != null && objectToActivate != null)
            {
                // Activate the specified GameObject
                objectToActivate.SetActive(false);
                gameObjectIsAcite = false;
            }
        }
    }
}
