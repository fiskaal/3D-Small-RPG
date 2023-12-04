using UnityEngine;

public class ActivateObjectOnTrigger : MonoBehaviour
{
    // Reference to the GameObject you want to activate
    public GameObject objectToActivate;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is the player
        if (other.CompareTag("Player"))
        {
            // Activate the specified GameObject
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }
        }
    }
}
