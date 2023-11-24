using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // Static reference to the WeaponManager instance
    public static WeaponManager Instance;

    // Strings to store the names of the equipped weapon and sheath
    public string EquippedWeaponName;
    public string EquippedSheathName;

    // The name of the specific weapon holder and sheath holder you want to find
    public string weaponHolderNameToFind = "SwordHolder";
    public string sheathHolderNameToFind = "SheathHolder";

    // Tag to identify the player GameObject
    public string playerTag = "Player";

    // Frequency of checking for the holders (in seconds)
    public float checkInterval = 1f;

    // References to the current holders
    private GameObject weaponHolder;
    private GameObject sheathHolder;

    // Reference to the DamageOfEverything script
    private DamageOfEverything damageScript;

    // Public variables to mirror the ones in DamageOfEverything
    public float weaponDamage;
    public float knockBackForce;
    public float lightingStrikeDamage;
    public float fireEnchantDamageBonus;
    public float lightningEnchantDamageBonus;
    public float enchantedWeaponDamage;

    private void Awake()
    {
        // Ensure there is only one instance of WeaponManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;  // Exit the method to avoid further execution
        }

        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
        {
            // Get the DamageOfEverything script attached to the player
            damageScript = player.GetComponent<DamageOfEverything>();

            if (damageScript == null)
            {
                Debug.LogError("DamageOfEverything script not found on the player GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player not found. Make sure the player has the tag: " + playerTag);
        }
    }


    // Start method to initiate the check for the holders
    private void Start()
    {
        InvokeRepeating("FindHolders", 0f, checkInterval);
    }

    // You can add other WeaponManager functionality here

    // Example method to change the equipped weapon
    public void EquipWeapon(string newWeaponName)
    {
        EquippedWeaponName = newWeaponName;

        // Update damage values in WeaponManager
        UpdateDamageValues();
    }

    // Example method to change the equipped sheath
    public void EquipSheath(string newSheathName)
    {
        EquippedSheathName = newSheathName;

        // Add additional logic for handling the sheath change if needed
    }

    // Method to find the weapon and sheath holders associated with the player
    private void FindHolders()
    {
        // Find the player GameObject using the tag
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
        {
            Debug.Log("Player found: " + player.name);

            // Find the weapon holder by name
            weaponHolder = FindObjectInHierarchyByName(player.transform, weaponHolderNameToFind);

            if (weaponHolder != null)
            {
                Debug.Log("Found weapon holder with name: " + weaponHolderNameToFind);

                // Search for the child with the same name as EquippedWeaponName
                Transform weaponChildToActivate = weaponHolder.transform.Find(EquippedWeaponName);

                if (weaponChildToActivate != null)
                {
                    // Deactivate all children
                    foreach (Transform child in weaponHolder.transform)
                    {
                        child.gameObject.SetActive(false);
                    }

                    // Activate the specific child
                    weaponChildToActivate.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Child with name " + EquippedWeaponName + " not found in " + weaponHolderNameToFind);
                }
            }
            else
            {
                Debug.LogWarning("Weapon holder not found with name: " + weaponHolderNameToFind);
            }

            // Find the sheath holder by name
            sheathHolder = FindObjectInHierarchyByName(player.transform, sheathHolderNameToFind);

            if (sheathHolder != null)
            {
                Debug.Log("Found sheath holder with name: " + sheathHolderNameToFind);

                // Search for the child with the same name as EquippedSheathName
                Transform sheathChildToActivate = sheathHolder.transform.Find(EquippedSheathName);

                if (sheathChildToActivate != null)
                {
                    // Deactivate all children
                    foreach (Transform child in sheathHolder.transform)
                    {
                        child.gameObject.SetActive(false);
                    }

                    // Activate the specific child
                    sheathChildToActivate.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Child with name " + EquippedSheathName + " not found in " + sheathHolderNameToFind);
                }
            }
            else
            {
                Debug.LogWarning("Sheath holder not found with name: " + sheathHolderNameToFind);
            }
        }
        else
        {
            Debug.LogWarning("Player not found. Make sure the player has the tag: " + playerTag);
        }

        UpdateDamageValues();

    }

    private GameObject FindObjectInHierarchyByName(Transform parent, string objectName)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child.name == objectName)
            {
                return child.gameObject;
            }
        }

        return null;
    }

    private void UpdateDamageValues()
    {
        // Update the values in DamageOfEverything with the corresponding variables from WeaponManager
        damageScript.weaponDamage = weaponDamage;
        damageScript.knockBackForce = knockBackForce;
        damageScript.lightingStrikeDamage = lightingStrikeDamage;
        damageScript.fireEnchantDamageBonus = fireEnchantDamageBonus;
        damageScript.lightningEnchantDamageBonus = lightningEnchantDamageBonus;
        damageScript.enchantedWeaponDamage = enchantedWeaponDamage;

        // You can perform additional logic or modifications here if needed
    }

}