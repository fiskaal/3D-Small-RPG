using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    // String variables for the weapon and sheath names
    public string weaponNameInput;
    public string sheathNameInput;

    // Reference to the button component
    private Button button;

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
        // Set the EquippedWeaponName and EquippedSheathName directly in the static WeaponManager
        WeaponManager.Instance.EquippedWeaponName = weaponNameInput;
        WeaponManager.Instance.EquippedSheathName = sheathNameInput;

        // Optional: Log a message to indicate that the names have been set
        Debug.Log("Equipped Weapon: " + weaponNameInput + ", Equipped Sheath: " + sheathNameInput);
    }
}
