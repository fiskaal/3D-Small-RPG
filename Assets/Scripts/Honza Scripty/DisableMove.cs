using UnityEngine;
using UnityEngine.InputSystem;
public class DisableMove : MonoBehaviour
{
    public string targetTag = "YourTargetTag"; // Set your specific tag here
    public PlayerInput playerInput;
    private void Update()
    {
        // Check if at least one GameObject with the specified tag is active in the scene
        bool anyObjectActive = GameObject.FindGameObjectWithTag(targetTag) != null;
        // Toggle the PlayerInput functionality based on the presence of active objects with the tag
        if (playerInput != null)
        {
            playerInput.enabled = !anyObjectActive;
        }
        else
        {
            Debug.LogWarning("PlayerInput component not assigned.");
        }
    }
}