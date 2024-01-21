using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Add this line for IEnumerator

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

            // Register the SceneLoaded method to be called when a scene is loaded
            SceneManager.sceneLoaded += SceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerEquipment = FindObjectOfType<PlayerEquipment>();

        if (playerEquipment == null)
            Debug.LogError("PlayerEquipment script not found in the scene.");

        StartCoroutine(SaveEquipmentStatesCoroutine()); // Start the coroutine to save equipment states
        LoadEquipmentStates();
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

    // Coroutine to save equipment states every 5 seconds
    private IEnumerator SaveEquipmentStatesCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f); // Wait for 5 seconds

            SaveEquipmentStates(); // Save equipment states to PlayerPrefs
        }
    }

    // Save equipment states to PlayerPrefs
    private void SaveEquipmentStates()
    {
        ArmorEquipmentItem[] externalItems = GetAllExternalItems();

        foreach (ArmorEquipmentItem externalItem in externalItems)
        {
            PlayerPrefs.SetInt(externalItem.variableName, externalItem.activateEquipment ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerEquipment = FindObjectOfType<PlayerEquipment>();

        if (playerEquipment == null)
            Debug.LogError("PlayerEquipment script not found in the scene.");
    }

    void OnDestroy()
    {
        // Unregister the SceneLoaded method to prevent memory leaks
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    [System.Serializable]
    public class ArmorEquipmentItem
    {
        public string variableName; // Name of the equipment item in PlayerEquipment
        public bool activateEquipment; // Bool to determine whether to activate or deactivate the variable in PlayerEquipment
        // Other fields if needed
    }

    // Load equipment states from PlayerPrefs
    private void LoadEquipmentStates()
    {
        ArmorEquipmentItem[] externalItems = GetAllExternalItems();

        foreach (ArmorEquipmentItem externalItem in externalItems)
        {
            int savedState = PlayerPrefs.GetInt(externalItem.variableName, 0);
            externalItem.activateEquipment = (savedState == 1);
        }
    }
}
