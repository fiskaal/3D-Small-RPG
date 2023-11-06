using UnityEngine;

public class BeamActivator : MonoBehaviour
{
    public string targetTag = "YourTag"; // Specify your target tag here
    public GameObject[] objectsToActivate;
    public float rangeThreshold = 10f; // Specify the range within which the player should not be present

    private bool isActive = false;
    private int currentIndex = 0;
    private float lastActivationTime = 0f;

    void Update()
    {
        // Find all game objects with the specified tag
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(targetTag);

        // Find the player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Check if there are 5 or fewer objects with the specified tag and the player is not within the range
        if (targetObjects.Length <= 5 && Vector3.Distance(transform.position, player.transform.position) > rangeThreshold)
        {
            // Start the activation loop if not active
            if (!isActive)
            {
                InvokeRepeating("ActivateObject", 0f, 10f);
                isActive = true;
            }
        }
        else
        {
            // Stop the activation loop if there are more than 5 objects with the specified tag or the player is within the range
            if (isActive)
            {
                CancelInvoke("ActivateObject");
                isActive = false;
            }
        }
    }

    void ActivateObject()
    {
        // Check if enough time has passed since the last activation
        if (Time.time - lastActivationTime >= 15f)
        {
            // Deactivate the current object
            if (objectsToActivate.Length > 0)
            {
                objectsToActivate[currentIndex].SetActive(false);

                // Increment index and loop if necessary
                currentIndex = (currentIndex + 1) % objectsToActivate.Length;

                // Activate the next object
                objectsToActivate[currentIndex].SetActive(true);

                // Update the last activation time
                lastActivationTime = Time.time;

                // Deactivate the object after 2 seconds
                Invoke("DeactivateObject", 2f);
            }
        }
    }

    void DeactivateObject()
    {
        // Deactivate the current object after 2 seconds
        if (objectsToActivate.Length > 0)
        {
            objectsToActivate[currentIndex].SetActive(false);
        }
    }
}
