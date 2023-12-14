using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    // Define equipment items
    public PlayerArmorEquipmentItem KnightHelmet;
    public PlayerArmorEquipmentItem KnightChestplate;
    public PlayerArmorEquipmentItem KnightLegs;
    public PlayerArmorEquipmentItem KnightBoots;

    // Method to update equipment state based on external script
    public void UpdateEquipmentState(string itemName, bool isEquipped)
    {
        switch (itemName)
        {
            case "KnightHelmet":
                UpdateEquipmentState(KnightHelmet, isEquipped);
                break;
            case "KnightChestplate":
                UpdateEquipmentState(KnightChestplate, isEquipped);
                break;
            case "KnightLegs":
                UpdateEquipmentState(KnightLegs, isEquipped);
                break;
            case "KnightBoots":
                UpdateEquipmentState(KnightBoots, isEquipped);
                break;
            default:
                Debug.LogWarning("Unknown equipment item: " + itemName);
                break;
        }
    }

    // Helper method to update equipment state and activate/deactivate objects
    private void UpdateEquipmentState(PlayerArmorEquipmentItem equipmentItem, bool isEquipped)
    {
        equipmentItem.isEquipped = isEquipped;

        // Activate/deactivate the item object and default object based on the equipped state
        if (isEquipped)
        {
            ActivateObject(equipmentItem.itemObject);
            DeactivateObject(equipmentItem.defaultObject);
        }
        else
        {
            ActivateObject(equipmentItem.defaultObject);
            DeactivateObject(equipmentItem.itemObject);
        }
    }

    // Helper method to activate an object if it's not null
    private void ActivateObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(true);
        }
    }

    // Helper method to deactivate an object if it's not null
    private void DeactivateObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }
}

[System.Serializable]
public class PlayerArmorEquipmentItem
{
    public bool isEquipped;
    public GameObject itemObject; // Reference to the actual game object for the equipment
    public GameObject defaultObject; // Reference to the default player body
}
