using UnityEngine;

public class BlueprintUnlock : MonoBehaviour
{
    // Index of the shop to set the bought status
    public int shopArrayIndex;
    public GameObject rewardPanel;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Set the bought status using the specified index
            ShopManager shopManager = FindObjectOfType<ShopManager>();
            if (shopManager != null)
            {
                shopManager.SetSwordBoughtStatus(shopArrayIndex, true);
            }
            else
            {
                Debug.LogError("ShopManager not found in the scene");
            }

            rewardPanel.SetActive(true);
            Time.timeScale = 0f;

            // Optionally, you can destroy the current object after activation
           //Destroy(gameObject);
        }
    }
}
