using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private bool hasKey = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Assuming "Player" tag is assigned to the player GameObject
            // Player has picked up the key
            hasKey = true;

            // Disable the key GameObject since it's been picked up
            gameObject.SetActive(false);

            // Find the chest by tag and set its state only if the player has the key
            if (hasKey)
            {
                GameObject chestObject = GameObject.FindGameObjectWithTag("Chest");
                if (chestObject != null)
                {
                    Chest chestScript = chestObject.GetComponent<Chest>();
                    if (chestScript != null)
                    {
                        chestScript.SetChestState(true);
                    }
                }
            }
        }
    }

    public bool HasKey()
    {
        return hasKey;
    }
}
