using UnityEngine;
using UnityEngine.UI;

public class EquipArmorButton : MonoBehaviour
{
    // Reference to the EquipmentManager script
    private EquipmentManager equipmentManager;

    // Enum to specify the equipment item
    public enum EquipmentItemType
    {
        Helmet,
        Chestplate,
        Legs,
        Boots
    }

    // Variable to store the selected equipment item
    public EquipmentItemType selectedEquipmentItem;

    void Start()
    {
        // Automatically find the EquipmentManager instance in the scene
        equipmentManager = FindObjectOfType<EquipmentManager>();

        if (equipmentManager != null)
        {
            // Find the button in the scene
            Button button = GetComponent<Button>();

            // Add a listener to the button click event
            button.onClick.AddListener(OnClickEquipItem);
        }
        else
        {
            Debug.LogError("EquipmentManager instance not found in the scene!");
        }
    }

    // Method to be called when the button is clicked
    void OnClickEquipItem()
    {
        if (equipmentManager != null)
        {
            // Set the selected equipment item to true based on the enum
            switch (selectedEquipmentItem)
            {
                case EquipmentItemType.Helmet:
                    equipmentManager.ExternalKnightHelmet.activateEquipment = true;
                    equipmentManager.EquipExternalItem(equipmentManager.ExternalKnightHelmet);
                    break;
                case EquipmentItemType.Chestplate:
                    equipmentManager.ExternalKnightChestplate.activateEquipment = true;
                    equipmentManager.EquipExternalItem(equipmentManager.ExternalKnightChestplate);
                    break;
                case EquipmentItemType.Legs:
                    equipmentManager.ExternalKnightLegs.activateEquipment = true;
                    equipmentManager.EquipExternalItem(equipmentManager.ExternalKnightLegs);
                    break;
                case EquipmentItemType.Boots:
                    equipmentManager.ExternalKnightBoots.activateEquipment = true;
                    equipmentManager.EquipExternalItem(equipmentManager.ExternalKnightBoots);
                    break;
                default:
                    Debug.LogError("Invalid EquipmentItemType");
                    break;
            }
        }
        else
        {
            Debug.LogError("EquipmentManager instance not found!");
        }
    }
}
