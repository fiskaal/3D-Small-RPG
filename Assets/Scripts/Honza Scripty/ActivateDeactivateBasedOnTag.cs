using UnityEngine;

public class ActivateDeactivateBasedOnTag : MonoBehaviour
{
    public string targetTag = "YourTargetTag"; // Set your specific tag here
    public GameObject objectToActivateDeactivate;

    void Update()
    {
        // Check if there is at least one GameObject with the specified tag
        bool atLeastOneObjectWithTag = GameObject.FindGameObjectWithTag(targetTag) != null;

        // Activate or deactivate the target GameObject based on the presence of objects with the specified tag
        objectToActivateDeactivate.SetActive(!atLeastOneObjectWithTag);
    }
}
