using UnityEngine;
using UnityEngine.UI;

public class PivotCentering : MonoBehaviour
{
    public Transform objectToMove; // The object you want to move
    public Transform targetObject; // The object whose center will be the target

    private void Start()
    {
        // Assuming you have a button component attached to the same GameObject
        Button button = GetComponent<Button>();

        if (button != null)
        {
            // Attach a method to the button's click event
            button.onClick.AddListener(MoveObjectToCenter);
        }
        else
        {
            Debug.LogError("Button component not found on the GameObject.");
        }
    }

    private void MoveObjectToCenter()
    {
        if (objectToMove != null && targetObject != null)
        {
            // Calculate the center position of the target object
            Vector3 targetCenter = targetObject.position;

            // Set the position of the object to the calculated center position
            objectToMove.position = targetCenter;

            Debug.Log("Object moved to the center of the target object without altering RectTransform properties.");
        }
        else
        {
            Debug.LogError("Please assign both objectToMove and targetObject in the inspector.");
        }
    }
}