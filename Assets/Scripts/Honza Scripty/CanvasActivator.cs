using UnityEngine;

public class CanvasActivator : MonoBehaviour
{
    public GameObject targetGameObject; // Assign the specific GameObject you want to activate in the Inspector
    private bool isPlayerInsideCollider;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideCollider = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideCollider = false;
        }
    }

    void Update()
    {
        // Check if the player is inside the collider and the "E" key is pressed
        if (isPlayerInsideCollider && Input.GetKeyDown(KeyCode.E))
        {
            if (targetGameObject != null)
            {
                targetGameObject.SetActive(true);
            }
        }
    }
}
