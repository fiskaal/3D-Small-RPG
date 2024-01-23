using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ClearSave : MonoBehaviour
{
    public Button clearPrefsButton;
    private SpellManager spellScript;
    private EquipmentManager armorScript;

    private void Start()
    {
        // Assuming you've attached this script to a button in the Unity Editor
        clearPrefsButton.onClick.AddListener(ClearPrefsButtonClick);
        spellScript = FindObjectOfType<SpellManager>();
        armorScript = FindObjectOfType<EquipmentManager>();
        clearPrefsButton.onClick.AddListener(ClearEverything);
        //clearPrefsButton.onClick.AddListener(RestartSpells);
    }

    private void ClearPrefsButtonClick()
    {
        // Find the GameManager instance and call the ClearPlayerPrefs method
        //WeaponManager.Instance.ClearPlayerPrefs();


        Debug.Log("PlayerPrefs cleared!");
    }

    void RestartSpells()
    {
        spellScript.RestartSpellStates();
    }

    void ClearEverything()
    {
        PlayerPrefs.DeleteAll();
        spellScript.RestartSpellStates();
        WeaponManager.Instance.ClearPlayerPrefs();
        armorScript.DeactivateAllExternalItems();
        ManagerPickups.soul = 0;
        PlayerPrefs.Save();
        spellScript.LoadSpellInfoFromPlayerPrefs();


    }

    public void SceneLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
