using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    // Public fields for the weapon attributes in the shop button
    public float weaponDamage;
    public float knockBackForce;
    public float lightingStrikeDamage;
    public float fireEnchantDamageBonus;
    public float lightningEnchantDamageBonus;
    public float enchantedWeaponDamage;

    // Public fields for the weapon and sheath names
    public string weaponNameInput;
    public string sheathNameInput;

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

    private void Start()
    {
        // Get the Button component on this GameObject
        button = GetComponent<Button>();

        // Attach the method to be called when the button is clicked
        button.onClick.AddListener(Equip);
    }

    // Method to be called when the button is clicked
    public void Equip()
    {
        // Check if the player has enough souls to activate
        if (ManagerPickups.soul >= soulCost)
        {
            // Deduct the soul cost from the resources
            ManagerPickups.UpdateSoulCount(-soulCost);

            // Set the values directly in the WeaponManager.Instance
            WeaponManager.Instance.weaponDamage = weaponDamage;
            WeaponManager.Instance.knockBackForce = knockBackForce;
            WeaponManager.Instance.lightingStrikeDamage = lightingStrikeDamage;
            WeaponManager.Instance.fireEnchantDamageBonus = fireEnchantDamageBonus;
            WeaponManager.Instance.lightningEnchantDamageBonus = lightningEnchantDamageBonus;
            WeaponManager.Instance.enchantedWeaponDamage = enchantedWeaponDamage;

            // Set the weapon and sheath names
            WeaponManager.Instance.EquippedWeaponName = weaponNameInput;
            WeaponManager.Instance.EquippedSheathName = sheathNameInput;

            // Optional: Log a message to indicate that the values have been set
            Debug.Log("ShopButton values set: " +
                "WeaponDamage: " + weaponDamage +
                ", KnockBackForce: " + knockBackForce +
                ", LightingStrikeDamage: " + lightingStrikeDamage +
                ", FireEnchantDamageBonus: " + fireEnchantDamageBonus +
                ", LightningEnchantDamageBonus: " + lightningEnchantDamageBonus +
                ", EnchantedWeaponDamage: " + enchantedWeaponDamage +
                ", WeaponNameInput: " + weaponNameInput +
                ", SheathNameInput: " + sheathNameInput);

            if (activatedObject != null)
            {
                activatedObject.SetActive(true);
            }

            // Deactivate the specified game object
            if (deactivatedObject != null)
            {
                deactivatedObject.SetActive(false);
            }
        }
        else
        {
            // Not enough souls, activate the warning message
            warningMessage.SetActive(true);
        }
    }
}
