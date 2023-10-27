using UnityEngine;

public class SelfDeactivator : MonoBehaviour
{
    private bool isActive = true;

    void Start()
    {
        // Call the CheckAndDeactivate method after 2 seconds
        Invoke("CheckAndDeactivate", 2f);
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
