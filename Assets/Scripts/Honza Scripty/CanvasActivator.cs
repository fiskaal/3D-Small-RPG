using UnityEngine;
using UnityEngine.InputSystem;

public class CanvasActivator : MonoBehaviour
{
    public GameObject targetGameObject; // Assign the specific GameObject you want to activate in the Inspector
    public GameObject iconObject; // Public reference to the IconObject, assign it in the Inspector
    private bool isPlayerInsideCollider;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideCollider = true;
            ActivateIconObject();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetGameObject != null)
            {
                targetGameObject.SetActive(false);
            }

            isPlayerInsideCollider = false;
            DeactivateIconObject();

        }
    }

    void Update()
    {
        // Check if the player is inside the collider and the "E" key is pressed
        if (isPlayerInsideCollider && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button1)))
        {
            if (targetGameObject != null)
            {
                if (targetGameObject.activeSelf != true)
                {
                    targetGameObject.SetActive(true);
                }
                else if (targetGameObject.activeSelf == true)
                {
                     targetGameObject.SetActive(false);
                }
            }
        }

        // Check if the player presses the Escape key to close the targetGameObject
        if ((Input.GetKeyDown(KeyCode.Escape)) || (Input.GetKeyDown(KeyCode.Joystick1Button2)))
        {
            if (targetGameObject != null)
            {
                targetGameObject.SetActive(false);
            }
        }
    }

    void ActivateIconObject()
    {
        if (iconObject != null)
        {
            iconObject.SetActive(true);
        }
    }

    void DeactivateIconObject()
    {
        if (iconObject != null)
        {
            iconObject.SetActive(false);
        }
    }
}
