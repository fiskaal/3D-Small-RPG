using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponManager : MonoBehaviour
{
    // Static reference to the GameManager instance
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

    public bool MagicShield;
    public bool Fireball;

    // PlayerPrefs keys
    //šedivý, protože WeaponHolderKey SheathHolderKey se nemìní, nejsou referencovaný, zustavají stejny, 2 liny vespod nepotøebné
    private const string WeaponHolderKey = "WeaponHolder";
    private const string SheathHolderKey = "SheathHolder";

    private const string WeaponDamageKey = "WeaponDamage";
    private const string KnockBackForceKey = "KnockBackForce";
    private const string LightingStrikeDamageKey = "LightingStrikeDamage";
    private const string FireEnchantDamageBonusKey = "FireEnchantDamageBonus";
    private const string LightningEnchantDamageBonusKey = "LightningEnchantDamageBonus";
    private const string EnchantedWeaponDamageKey = "EnchantedWeaponDamage";

    // Default values
    private const string DefaultEquippedWeaponName = "Hatchet";
    private const string DefaultEquippedSheathName = "Hatchet";

    // Additional PlayerPrefs keys
    private const string EquippedWeaponNameKey = "EquippedWeaponName";
    private const string EquippedSheathNameKey = "EquippedSheathName";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            SetDefaultValues();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        {
            damageScript = FindObjectOfType<DamageOfEverything>();

            if (damageScript == null)
            {
                Debug.LogError("DamageOfEverything script not found in the scene.");
            }
            else
            {
                // Do something with the found damageScript instance
            }
        }

    }

    private void Start()
    {
        InvokeRepeating("FindHolders", 0f, checkInterval);
        FindHolders();

        // Load saved values or set defaults
       LoadValues();
    }

    public void EquipWeapon(string newWeaponName)
    {
        EquippedWeaponName = newWeaponName;
        UpdateDamageValues();
    }

    public void EquipSheath(string newSheathName)
    {
        EquippedSheathName = newSheathName;
    }

    public void FindHolders()
    {
        GameObject player = GameObject.Find("Player");

        if (player != null)
        {
            weaponHolder = FindObjectInHierarchyByName(player.transform, weaponHolderNameToFind);
            sheathHolder = FindObjectInHierarchyByName(player.transform, sheathHolderNameToFind);

            if (weaponHolder != null)
            {
                Debug.Log("Found weapon holder with name: " + weaponHolderNameToFind);
                ActivateChildBasedOnEquipped(weaponHolder, EquippedWeaponName);

                UpdateDamageValues();
            }
            else
            {
                Debug.LogWarning("Weapon holder not found with name: " + weaponHolderNameToFind);
            }

            if (sheathHolder != null)
            {
                Debug.Log("Found sheath holder with name: " + sheathHolderNameToFind);
                ActivateChildBasedOnEquipped(sheathHolder, EquippedSheathName);
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
    }

    private void UpdateDamageValues()
    {
        damageScript.weaponDamage = weaponDamage;
        damageScript.knockBackForce = knockBackForce;
        damageScript.lightingStrikeDamage = lightingStrikeDamage;
        damageScript.fireEnchantDamageBonus = fireEnchantDamageBonus;
        damageScript.lightningEnchantDamageBonus = lightningEnchantDamageBonus;
        damageScript.enchantedWeaponDamage = enchantedWeaponDamage;
    }

    private void OnDestroy()
    {
        SaveValues();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        damageScript = FindObjectOfType<DamageOfEverything>();

        if (damageScript == null)
        {
            Debug.LogError("DamageOfEverything script not found in the scene.");
        }
        else
        {
            // Do something with the found damageScript instance
        }

        //LoadValues();
        FindHolders();
        UpdateCharacterBlockIsUpgraded();
        UpdateFireballIsUpgraded();
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

    private void ActivateChildBasedOnEquipped(GameObject holder, string equippedItemName)
    {
        if (holder != null)
        {
            Transform childToActivate = holder.transform.Find(equippedItemName);

            if (childToActivate != null)
            {
                foreach (Transform child in holder.transform)
                {
                    child.gameObject.SetActive(false);
                }

                childToActivate.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Child with name " + equippedItemName + " not found in " + holder.name);
            }
        }
    }

    public void SaveValues()
    {
        PlayerPrefs.SetString(EquippedWeaponNameKey, EquippedWeaponName);
        PlayerPrefs.SetString(EquippedSheathNameKey, EquippedSheathName);

        PlayerPrefs.SetFloat(WeaponDamageKey, weaponDamage);
        PlayerPrefs.SetFloat(KnockBackForceKey, knockBackForce);
        PlayerPrefs.SetFloat(LightingStrikeDamageKey, lightingStrikeDamage);
        PlayerPrefs.SetFloat(FireEnchantDamageBonusKey, fireEnchantDamageBonus);
        PlayerPrefs.SetFloat(LightningEnchantDamageBonusKey, lightningEnchantDamageBonus);
        PlayerPrefs.SetFloat(EnchantedWeaponDamageKey, enchantedWeaponDamage);

        PlayerPrefs.SetInt("MagicShield", MagicShield ? 1 : 0);
        PlayerPrefs.SetInt("Fireball", Fireball ? 1 : 0);

        PlayerPrefs.Save();
    }

    public void LoadValues()
    {
        // Load values from PlayerPrefs, or use defaults if not present
        EquippedWeaponName = PlayerPrefs.GetString(EquippedWeaponNameKey, DefaultEquippedWeaponName);
        EquippedSheathName = PlayerPrefs.GetString(EquippedSheathNameKey, DefaultEquippedSheathName);

        weaponDamage = PlayerPrefs.GetFloat(WeaponDamageKey, 1f);
        knockBackForce = PlayerPrefs.GetFloat(KnockBackForceKey, 0f); // Default to 0 if not present
        lightingStrikeDamage = PlayerPrefs.GetFloat(LightingStrikeDamageKey, 0f); // Default to 0 if not present
        fireEnchantDamageBonus = PlayerPrefs.GetFloat(FireEnchantDamageBonusKey, 0f); // Default to 0 if not present
        lightningEnchantDamageBonus = PlayerPrefs.GetFloat(LightningEnchantDamageBonusKey, 0f); // Default to 0 if not present
        enchantedWeaponDamage = PlayerPrefs.GetFloat(EnchantedWeaponDamageKey, 0f); // Default to 0 if not present

        MagicShield = PlayerPrefs.GetInt("MagicShield", 0) == 1;
        Fireball = PlayerPrefs.GetInt("Fireball", 0) == 1;
        UpdateCharacterBlockIsUpgraded();
        UpdateFireballIsUpgraded();

        FindHolders();
    }

    public void ClearPlayerPrefs()
    {
        // Optionally clear all PlayerPrefs (use with caution)
        PlayerPrefs.DeleteAll();

        // Save PlayerPrefs after clearing keys
        PlayerPrefs.Save();
        SetDefaultValues();
    }

    public void SetDefaultValues()
    {
        // Set default values for weapon-related variables
        weaponDamage = 1.0f;
        knockBackForce = 0.0f; // Set your default value here
        lightingStrikeDamage = 3.0f; // Set your default value here
        fireEnchantDamageBonus = 0.0f; // Set your default value here
        lightningEnchantDamageBonus = 0.0f; // Set your default value here
        enchantedWeaponDamage = 0.0f; // Set your default value here

        // Set default values for weapon names and holders
        EquippedWeaponName = "Hatchet";
        EquippedSheathName = "Hatchet";
        weaponHolderNameToFind = "SwordHolder"; // Set your default value here
        sheathHolderNameToFind = "SheathHolder"; // Set your default value here

        MagicShield = false; // deactivating the magic shield bought status
        Fireball = false;

        // Set default value for the player tag
        playerTag = "Player"; // Set your default value here
    }

    public void UpdateCharacterBlockIsUpgraded()
    {
        // Find the Player GameObject by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Check if the Player GameObject exists and has the Character script
        if (player != null)
        {
            Character characterScript = player.GetComponent<Character>();

            // Check if the Character script is found
            if (characterScript != null)
            {
                // Update blockIsUgraded in Character script based on MagicShield
                characterScript.blockIsUgraded = MagicShield;
            }
            else
            {
                Debug.LogWarning("Character script not found on the Player GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Player not found. Make sure the player has the tag: Player");
        }
    }

    public void UpdateFireballIsUpgraded()
    {

            Character characterScript = FindObjectOfType<Character>();

            // Check if the Character script is found
            if (characterScript != null)
            {
                // Update blockIsUgraded in Character script based on MagicShield
                characterScript.fireBallIsActive = Fireball;
            }
            else
            {
                Debug.LogWarning("Character script not found on the Player GameObject.");
            }

    }



}
