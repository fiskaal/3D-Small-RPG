using UnityEngine;

public class CursorVisibilityController : MonoBehaviour
{
    public GameObject[] objectsToCheck; // Array of game objects to check
    public GameObject parentObject; // Specific game object to check its children
    public UserCamera userCamera; // Reference to the UserCamera script

    void Update()
    {
        bool shouldShowCursor = false;
        bool shouldFreezeCamera = false;

        // Check if any object in the array is active
        foreach (GameObject obj in objectsToCheck)
        {
            if (obj.activeSelf)
            {
                shouldShowCursor = true;
                shouldFreezeCamera = true;
                break;
            }
        }

        // Check if any child of the specific game object is active
        if (parentObject != null)
        {
            foreach (Transform child in parentObject.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    shouldShowCursor = true;
                    shouldFreezeCamera = true;
                    break;
                }
            }
        }

        // Check if the Ctrl key is being held down
        bool isCtrlPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        // Show cursor if any object in the array or any child of the specific object is active or Ctrl key is held down
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

        // Freeze camera rotation if any object in the array or any child of the specific object is active
        userCamera.enabled = !shouldFreezeCamera;
    }
}
