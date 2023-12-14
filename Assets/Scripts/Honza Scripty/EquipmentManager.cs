using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    // Define external script's equipment items
    public ArmorEquipmentItem ExternalKnightHelmet;
    public ArmorEquipmentItem ExternalKnightChestplate;
    public ArmorEquipmentItem ExternalKnightLegs;
    public ArmorEquipmentItem ExternalKnightBoots;

    // Reference to the PlayerEquipment script
    public PlayerEquipment playerEquipment;

    // Singleton instance
    public static EquipmentManager instance;

    void Awake()
    {
        // Ensure there's only one instance of EquipmentManager across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Find the player GameObject using the "Player" tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // Get the PlayerEquipment script attached to the player GameObject
        playerEquipment = playerObject.GetComponent<PlayerEquipment>();

        if (playerEquipment == null)
        {
            Debug.LogError("PlayerEquipment script not found on the player GameObject.");
        }
    }

    void Update()
    {
        // Example usage: Equipping all external items
        EquipAllExternalItems();
    }

    // Helper method to update equipment state in PlayerEquipment for all external items
    public void EquipAllExternalItems()
    {
        EquipExternalItem(ExternalKnightHelmet);
        EquipExternalItem(ExternalKnightChestplate);
        EquipExternalItem(ExternalKnightLegs);
        EquipExternalItem(ExternalKnightBoots);
    }

    // Helper method to update equipment state in PlayerEquipment
    public void EquipExternalItem(ArmorEquipmentItem externalItem)
    {
        if (playerEquipment != null)
        {
            // Update playerEquipment's item state based on the specified external item
            playerEquipment.UpdateEquipmentState(externalItem.variableName, externalItem.activateEquipment);
        }
        else
        {
            Debug.LogWarning("PlayerEquipment script not found!");
        }
    }

    public ArmorEquipmentItem[] GetAllExternalItems()
    {
        ArmorEquipmentItem[] externalItems = {
            ExternalKnightHelmet,
            ExternalKnightChestplate,
            ExternalKnightLegs,
            ExternalKnightBoots
            // Add more items if needed
        };

        return externalItems;
    }

    [System.Serializable]
    public class ArmorEquipmentItem
    {
        public string variableName; // Name of the equipment item in PlayerEquipment
        public bool activateEquipment; // Bool to determine whether to activate or deactivate the variable in PlayerEquipment
        // Other fields if needed
    }
}
