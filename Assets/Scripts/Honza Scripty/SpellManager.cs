using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class SpellInfo
{
    public string childObjectName;
    public bool spellBought;
}

public class SpellManager : MonoBehaviour
{
    private static SpellManager instance;
    public List<SpellInfo> spellInfoList = new List<SpellInfo>();
    public string targetTag = "YourTargetTag";
    public string targetObjectName = "YourTargetObjectName";

    // Make childObjectName public
    public static string childObjectName;

    private const string PlayerPrefsKey = "SpellInfoList";

    void Awake()
    {
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
        LocateGameObject();
        LoadSpellInfoFromPlayerPrefs(); // Load saved data on start
        SetInitialSpellBoughtStatus();

        // Start the coroutine to save data every 5 seconds
        StartCoroutine(SaveDataCoroutine());
    }

    void LocateGameObject()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in objectsWithTag)
        {
            if (obj.name == targetObjectName)
            {
                Debug.Log("Found the target game object: " + obj.name);

                ActivateChildObjects(obj); // Activate child objects and set SpellBought bool

                return;
            }
        }

        Debug.LogWarning("Target game object not found in the scene.");
    }

    void ActivateChildObjects(GameObject parentObject)
    {
        foreach (SpellInfo spellInfo in spellInfoList)
        {
            Transform child = parentObject.transform.Find(spellInfo.childObjectName);

            if (child != null)
            {
                BooleanSpell booleanSpell = child.GetComponent<BooleanSpell>();

                // If BooleanSpell exists, update SpellBought
                if (booleanSpell != null)
                {
                    booleanSpell.SpellBought = spellInfo.spellBought;

                    // Deactivate the child object
                    child.gameObject.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("BooleanSpell component not found on child object: " + spellInfo.childObjectName);
                }
            }
            else
            {
                Debug.LogWarning("Child object not found: " + spellInfo.childObjectName);
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LocateGameObject();
    }

    public void SetSpellBoughtStatus(int spellIndex, bool isBought)
    {
        if (spellIndex >= 0 && spellIndex < spellInfoList.Count)
        {
            spellInfoList[spellIndex].spellBought = isBought;
            SaveSpellInfoToPlayerPrefs(); // Save the updated data
        }
        else
        {
            Debug.LogError("Invalid spellIndex: " + spellIndex);
        }
    }

    void SetInitialSpellBoughtStatus()
    {
        // Example: Set the first and third elements to true
        SetSpellBoughtStatus(1, true);
        SetSpellBoughtStatus(2, true);
        // You can add more lines to set other indices as needed
    }

    // Save spellInfoList to PlayerPrefs
    void SaveSpellInfoToPlayerPrefs()
    {
        string json = JsonUtility.ToJson(new SerializableSpellInfoList(spellInfoList));
        PlayerPrefs.SetString(PlayerPrefsKey, json);
        PlayerPrefs.Save();
        LoadSpellInfoFromPlayerPrefs();
    }

    // Load spellInfoList from PlayerPrefs
    public void LoadSpellInfoFromPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            string json = PlayerPrefs.GetString(PlayerPrefsKey);
            SerializableSpellInfoList serializedSpellInfoList = JsonUtility.FromJson<SerializableSpellInfoList>(json);

            if (serializedSpellInfoList != null)
            {
                spellInfoList = serializedSpellInfoList.spellInfoList;
            }
        }
    }

    // Coroutine to save data every 5 seconds
    IEnumerator SaveDataCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            SaveSpellInfoToPlayerPrefs();
        }
    }

    // Helper class for serialization
    [System.Serializable]
    private class SerializableSpellInfoList
    {
        public List<SpellInfo> spellInfoList;

        public SerializableSpellInfoList(List<SpellInfo> spellInfoList)
        {
            this.spellInfoList = spellInfoList;
        }
    }

    public void RestartSpellStates()
    {
        SetSpellBoughtStatus(3, false);
        SetSpellBoughtStatus(4, false);
    }
}
