using UnityEngine;

public class DeactivateGrandparentChildren : MonoBehaviour
{
    // Function to deactivate all child objects of the grandparent
    void DeactivateGrandparentChildObjects(Transform grandparent)
    {
        foreach (Transform child in grandparent)
        {
            child.gameObject.SetActive(false);
        }
    }

    // Callback function for the button click event
    public void OnButtonClick()
    {
        // Get the grandparent object of the button
        Transform grandparent = transform.parent.parent;

        // Deactivate all child objects of the grandparent
        if (grandparent != null)
        {
            DeactivateGrandparentChildObjects(grandparent);
        }
        else
        {
            Debug.LogWarning("Button does not have a grandparent object!");
        }
    }
}
