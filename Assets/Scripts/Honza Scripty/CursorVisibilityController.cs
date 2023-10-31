using UnityEngine;

public class CursorVisibilityController : MonoBehaviour
{
    public GameObject[] objectsToCheck;

    void Update()
    {
        bool shouldShowCursor = false;

        // Check if any specific game object in the array is active
        foreach (GameObject obj in objectsToCheck)
        {
            if (obj.activeSelf)
            {
                shouldShowCursor = true;
                break;
            }
        }

        // Check if the Ctrl key is being held down
        bool isCtrlPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        // Show cursor if any object is active or Ctrl key is held down
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
    }
}