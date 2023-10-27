using UnityEngine;

public class SelfDeactivator : MonoBehaviour
{
    private bool isActive = true;
    private float deactivationTime = 5f; // Set the deactivation time to 5 seconds

    void Start()
    {
        // Call the CheckAndDeactivate method after 2 seconds
        Invoke("CheckAndDeactivate", 2f);
    }

    void Update()
    {
        // Check if the object is still active after the specified deactivation time
        if (isActive)
        {
            deactivationTime -= Time.deltaTime;

            if (deactivationTime <= 0f)
            {
                // Deactivate the object
                gameObject.SetActive(false);
                Debug.Log("Object deactivated after 5 seconds.");
            }
        }
    }

    void CheckAndDeactivate()
    {
        if (isActive)
        {
            // Deactivate the object
            gameObject.SetActive(false);
            Debug.Log("Object deactivated after 2 seconds.");
        }
        else
        {
            Debug.Log("Object is already inactive.");
        }
    }
}
