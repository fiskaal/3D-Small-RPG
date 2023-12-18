using UnityEngine;

public class ActiveOnCollision : MonoBehaviour
{
    public GameObject objectToActivate; // Reference to the game object to activate

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            objectToActivate.SetActive(true);
        }
    }
}
