using UnityEngine;

public class MagicShieldButton : MonoBehaviour
{
    private WeaponManager weaponManager;

    // Soul cost required for activation
    public int soulCost = 10;

    // Reference to the warning message game object
    public GameObject warningMessage;
    public GameObject ObjectDeactivate;
    public GameObject ObjectActivate;
    // Index of the shop to set the bought status
    public int shopArrayIndex;

    private void Start()
    {
        // Find the WeaponManager at the start to avoid repeated Find calls
        weaponManager = FindObjectOfType<WeaponManager>();

        if (weaponManager == null)
        {
            Debug.LogWarning("WeaponManager not found in the scene.");
        }
    }

    public void OnMagicShieldButtonClick()
    {
        // Check if the player has enough souls to activate
        if (ManagerPickups.soul >= soulCost)
        {
            // Check if the WeaponManager is found
            if (weaponManager != null)
            {
                // Set the MagicShield bool to true
                weaponManager.MagicShield = true;

                ManagerPickups.UpdateSoulCount(-soulCost);

                // Optionally, you can save the updated value to PlayerPrefs
                weaponManager.SaveValues();
                weaponManager.LoadValues();
                ObjectDeactivate.SetActive(false);
                ObjectActivate.SetActive(true);

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
                Debug.LogWarning("WeaponManager not found. Make sure WeaponManager is in the scene.");
            }
        }
        else
        {
            // Not enough souls, activate the warning message
            warningMessage.SetActive(true);
        }
    }
}
