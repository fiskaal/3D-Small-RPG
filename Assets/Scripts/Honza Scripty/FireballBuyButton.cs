using UnityEngine;
using UnityEngine.UI;

public class FireballBuyButton : MonoBehaviour
{
    // Reference to the game object to activate when the player has enough souls
    public GameObject activatedObject;

    // Reference to the game object to deactivate when the player has enough souls
    public GameObject deactivatedObject;

    // Reference to the button component
    private Button button;

    // Soul cost required for activation
    public int soulCost = 10;

    // Reference to the warning message game object
    public GameObject warningMessage;

    // Index of the shop to set the bought status
    public int shopArrayIndex;

    private WeaponManager weaponManager;

    private void Start()
    {
        // Get the Button component on this GameObject
        button = GetComponent<Button>();


        // Find the WeaponManager at the start to avoid repeated Find calls
        weaponManager = FindObjectOfType<WeaponManager>();

        if (weaponManager == null)
        {
            Debug.LogWarning("WeaponManager not found in the scene.");
        }
    }

    // Method to be called when the button is clicked
    private void OnFireballBuyButtonClick()
    {
        // Check if the player has enough souls to activate
        if (ManagerPickups.soul >= soulCost)
        {
            // Deduct the soul cost from the resources
            ManagerPickups.UpdateSoulCount(-soulCost);

            if (activatedObject != null)
            {
                activatedObject.SetActive(true);
            }

            // Deactivate the specified game object
            if (deactivatedObject != null)
            {
                deactivatedObject.SetActive(false);
            }

            weaponManager.Fireball = true;
            weaponManager.UpdateFireballIsUpgraded();

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
        }
        else
        {
            // Not enough souls, activate the warning message
            warningMessage.SetActive(true);
        }
    }
}
