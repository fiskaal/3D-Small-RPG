using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{
    public Button saveButton;

    private void Start()
    {
        // Assuming you've attached this script to a button in the Unity Editor
        saveButton.onClick.AddListener(SaveButtonClick);
    }

    private void SaveButtonClick()
    {
        // Find the GameManager instance and call the SaveValues method
        WeaponManager.Instance.SaveValues();
        Debug.Log("Game values saved!");
    }
}
