using UnityEngine;

public class CursorVisibilityController : MonoBehaviour
{
    private bool isCtrlPressed = false;

    void Update()
    {
        // Check if the Ctrl key is being held down
        bool ctrlKeyPressedThisFrame = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        // If Ctrl key is pressed this frame, set the flag to true
        if (ctrlKeyPressedThisFrame)
        {
            isCtrlPressed = true;
        }

        // Set cursor visibility based on Ctrl key state
        Cursor.visible = isCtrlPressed;

        // Lock cursor if it's not visible
        if (!Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        // Reset the Ctrl key flag at the end of the frame
        isCtrlPressed = false;
    }
}
