using UnityEngine;

public class SpellBuyButton : MonoBehaviour
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
    // Index of the sword to set the bought status
    public int spellArrayIndex;

    private void Start()
    {
        // Find the WeaponManager at the start to avoid repeated Find calls
        weaponManager = FindObjectOfType<WeaponManager>();

        if (weaponManager == null)
        {
            Debug.LogWarning("WeaponManager not found in the scene.");
        }
    }

    public void OnSpellBuyButtonClick()
    {
        // Check if the player has enough souls to activate
        if (ManagerPickups.soul >= soulCost)
        {
            // Check if the WeaponManager is found
            if (weaponManager != null)
            {
                // Deduct the soul cost from the resources
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

                // Set the spell bought status using the specified index
                SpellManager spellManager = FindObjectOfType<SpellManager>();
                if (spellManager != null)
                {
                    // Assuming that SetSpellBoughtStatus is a method in SpellManager
                    spellManager.SetSpellBoughtStatus(spellArrayIndex, true);
                }
                else
                {
                    Debug.LogError("SpellManager not found in the scene");
                }
            }
            else
            {
                Debug.LogWarning("WeaponManager not found. Make sure WeaponManager is in the scene.");
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
