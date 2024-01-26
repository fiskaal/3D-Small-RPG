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
            // Not enough souls, instantiate the warning message
            if (warningMessage != null)
            {
                // Get the Canvas component (adjust this line based on how your Canvas is set up)
                Canvas canvas = FindObjectOfType<Canvas>();

                if (canvas != null)
                {
                    // Instantiate the warning message as a child of the Canvas
                    GameObject newWarningMessage = Instantiate(warningMessage, canvas.transform);
                    // Optionally, you can set the local position within the Canvas
                    // newWarningMessage.transform.localPosition = someLocalPosition;
                }
                else
                {
                    Debug.LogError("Canvas not found in the scene. Make sure you have a Canvas component.");
                }
            }
            else
            {
                Debug.LogError("Warning message prefab not assigned to the UnlockButton script.");
            }
        }
    }
}
