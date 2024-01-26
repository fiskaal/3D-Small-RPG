using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SwordInfo
{
    public string PlayerPrefsKey; // Unique key for PlayerPrefs
    public bool isBought;
    public string boughtObjectName;
    public string notBoughtObjectName;
}

public class ShopManager : MonoBehaviour
{
    // Static reference to the ShopManager instance
    private static ShopManager instance;

    // List of swords info
    public List<SwordInfo> swordsInfo = new List<SwordInfo>();

    // Awake is called before the Start method
    void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            // If not, set the instance to this
            instance = this;

            // Make the object persistent between scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }

        // Load sword status from PlayerPrefs
        LoadSwordStatus();
    }

    void Update()
    {
        foreach (var sword in swordsInfo)
        {
            if (sword.isBought)
            {
                ActivateSword(sword.boughtObjectName, sword.notBoughtObjectName);
            }
            else
            {
                DeactivateSword(sword.boughtObjectName, sword.notBoughtObjectName);
            }
        }
    }

    private void Start()
    {

    }

    void ActivateSword(string boughtObjectName, string notBoughtObjectName)
    {
        GameObject boughtObject = GameObject.Find(boughtObjectName);
        GameObject notBoughtObject = GameObject.Find(notBoughtObjectName);

        if (boughtObject != null)
        {
            boughtObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Bought object not found: " + boughtObjectName);
        }

        if (notBoughtObject != null)
        {
            notBoughtObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Not bought object not found: " + notBoughtObjectName);
        }
    }

    void DeactivateSword(string boughtObjectName, string notBoughtObjectName)
    {
        GameObject boughtObject = GameObject.Find(boughtObjectName);
        GameObject notBoughtObject = GameObject.Find(notBoughtObjectName);

        if (boughtObject != null)
        {
            boughtObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Bought object not found: " + boughtObjectName);
        }

        if (notBoughtObject != null)
        {
            notBoughtObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Not bought object not found: " + notBoughtObjectName);
        }
    }

    // Method to set the isBought property for a specific sword
    public void SetSwordBoughtStatus(int swordIndex, bool isBought)
    {
        if (swordIndex >= 0 && swordIndex < swordsInfo.Count)
        {
            swordsInfo[swordIndex].isBought = isBought;

            // Save sword status to PlayerPrefs
            SaveSwordStatus();
        }
        else
        {
            Debug.LogError("Invalid sword index: " + swordIndex);
        }
    }

    // Load sword status from PlayerPrefs
    void LoadSwordStatus()
    {
        foreach (var sword in swordsInfo)
        {
            // Use PlayerPrefsKey as a unique key for each sword
            sword.isBought = PlayerPrefs.GetInt(sword.PlayerPrefsKey, 0) == 1;
        }
    }

    // Save sword status to PlayerPrefs
    void SaveSwordStatus()
    {
        foreach (var sword in swordsInfo)
        {
            // Use PlayerPrefsKey as a unique key for each sword
            PlayerPrefs.SetInt(sword.PlayerPrefsKey, sword.isBought ? 1 : 0);
        }

        // Save changes
        PlayerPrefs.Save();
    }

    public void ResetSwordStatus()
    {
        foreach (var sword in swordsInfo)
        {
            sword.isBought = false;
        }

        // Save the updated sword status to PlayerPrefs
        SaveSwordStatus();
    }
}
