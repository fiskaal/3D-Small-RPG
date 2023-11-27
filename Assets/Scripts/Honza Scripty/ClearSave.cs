using UnityEngine;
using UnityEngine.UI;

public class ClearSave : MonoBehaviour
{
    public Button clearPrefsButton;

    private void Start()
    {
        // Assuming you've attached this script to a button in the Unity Editor
        clearPrefsButton.onClick.AddListener(ClearPrefsButtonClick);
    }

    private void ClearPrefsButtonClick()
    {
        // Find the GameManager instance and call the ClearPlayerPrefs method
        WeaponManager.Instance.ClearPlayerPrefs();
        Debug.Log("PlayerPrefs cleared!");
    }
}
