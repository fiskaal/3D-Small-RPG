using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public class SpellInfo
{
    public string childObjectName;
    public bool spellBought;
}

public class SpellManager : MonoBehaviour
{
    // Static reference to the SpellManager instance
    private static SpellManager instance;

    // List of spell info
    public List<SpellInfo> spellInfoList = new List<SpellInfo>();

    public string targetTag = "YourTargetTag";
    public string targetObjectName = "YourTargetObjectName";

    // Make childObjectName public
    public static string childObjectName;

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
    }

    void Start()
    {
        LocateGameObject();

        SetInitialSpellBoughtStatus();
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
        // Check if the spellIndex is within the valid range
        if (spellIndex >= 0 && spellIndex < spellInfoList.Count)
        {
            spellInfoList[spellIndex].spellBought = isBought;
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


}
