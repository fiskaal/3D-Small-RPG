using UnityEngine;

public class CursorVisibilityController : MonoBehaviour
{
    public string[] objectTags; // Specify the tags to check
    private UserCamera userCamera; // Reference to the UserCamera script

    void Start()
    {
        // Find the UserCamera by tag
        GameObject cameraObject = GameObject.FindWithTag("MainCamera");
        if (cameraObject != null)
        {
            userCamera = cameraObject.GetComponent<UserCamera>();
        }
        else
        {
            Debug.LogError("MainCamera not found in the scene!");
        }
    }

    void Update()
    {
        bool shouldShowCursor = false;
        bool shouldFreezeCamera = false;

        // Check if any object with the specified tags is active
        foreach (string tag in objectTags)
        {
            GameObject[] objectsToCheck = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objectsToCheck)
            {
                if (obj.activeSelf)
                {
                    shouldShowCursor = true;
                    shouldFreezeCamera = true;
                    break;
                }
            }
        }

        // Check if the Ctrl key is being held down
        bool isCtrlPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        // Show cursor if any object with the specified tags is active or Ctrl key is held down
        Cursor.visible = shouldShowCursor || isCtrlPressed;

        // Lock cursor if it's not visible
        if (!Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        // Freeze camera rotation if any object with the specified tags is active
        if (userCamera != null)
        {
            userCamera.enabled = !shouldFreezeCamera;
        }
    }
}
