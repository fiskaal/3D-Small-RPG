using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SpellInfo
{
    public string childObjectName;
    public bool spellBought;
}

public class SpellManager : MonoBehaviour
{
    public string targetTag = "YourTargetTag";
    public string targetObjectName = "YourTargetObjectName";

    public SpellInfo[] spellArray;

    private static SpellManager instance;

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
        foreach (SpellInfo spellInfo in spellArray)
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
}
