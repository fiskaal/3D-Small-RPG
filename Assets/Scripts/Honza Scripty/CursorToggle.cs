using UnityEngine;

public class CursorToggle : MonoBehaviour
{
    // Variable to hold the cursor visibility state
    public bool isCursorVisible = true;

    // Method to be called when the button is clicked
    public void ToggleCursorVisibility()
    {
        // Toggle the boolean variable
        isCursorVisible = !isCursorVisible;

        // Set the cursor visibility based on the updated boolean value
        Cursor.visible = isCursorVisible;
    }
}
