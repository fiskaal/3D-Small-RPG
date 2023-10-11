using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Find the main camera in the scene
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Ensure the sprite is always facing the camera on the Y-axis only
        if (mainCamera != null)
        {
            Vector3 lookPos = mainCamera.transform.position - transform.position;
            lookPos.y = 0; // Restrict rotation on the Y-axis only
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;
        }
        else
        {
            Debug.LogWarning("Main camera not found in the scene. Make sure you have a camera tagged as 'MainCamera'.");
        }
    }
}
