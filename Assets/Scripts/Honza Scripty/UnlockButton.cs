using UnityEngine;
using UnityEngine.UI;

public class UnlockButton : MonoBehaviour
{
    // Reference to the game object to activate when the player has enough souls
    public GameObject activatedObject;

    // Reference to the game object to deactivate when the player has enough souls
    public GameObject deactivatedObject;

    // Reference to the button component
    private Button button;
    private ShopButton equipScript;

    // Soul cost required for activation
    public int soulCost = 10;

    public GameObject weaponEquipButtonObject;
    public GameObject equipArmorButton;

    // Reference to the warning message game object
    public GameObject warningMessage;

    // Index of the shop to set the bought status
    public int shopArrayIndex;


    private void Start()
    {
        // Get the Button component on this GameObject
        button = GetComponent<Button>();

        // Attach the method to be called when the button is clicked
        button.onClick.AddListener(Equip);
    }

    // Method to be called when the button is clicked
    private void Equip()
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

            if (weaponEquipButtonObject != null)
            {
                EquipSword();
            }

            if (equipArmorButton != null)
            {
                EquipArmor();
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

    void EquipSword()
    {
        // equip the actual weapon object using reference to the equip button
        equipScript = weaponEquipButtonObject.GetComponent<ShopButton>();
        if (equipScript != null)
        {
            equipScript.Equip();
        }
        else
        {
            Debug.LogWarning("ShopButton, the actual equip script not found");
        }

        // Find the WeaponManager in the scene
        WeaponManager weaponManager = FindObjectOfType<WeaponManager>();

        // If the weaponManager is found, call the FindHolders method
        if (weaponManager != null)
        {
            weaponManager.FindHolders();
        }
        else
        {
            Debug.LogError("WeaponManager not found. Make sure the script is attached to a GameObject with WeaponManager.");
        }
    }

    void EquipArmor()
    {
        EquipArmorButton equipArmorScript = equipArmorButton.GetComponent<EquipArmorButton>();
        if (equipArmorScript != null)
        {
            equipArmorScript.OnClickEquipItem();
        }
        else
        {
            Debug.LogError("EquipArmorButton script was not found ");
        }
    }
}
