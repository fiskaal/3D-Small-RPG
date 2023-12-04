using UnityEngine;
using UnityEngine.UI;

public class LoadData : MonoBehaviour
{
    public Button loadButton;

    private void Start()
    {
        // Assuming you've attached this script to a button in the Unity Editor
        loadButton.onClick.AddListener(LoadButtonClick);
    }

    private void LoadButtonClick()
    {
        // Find the GameManager instance and call the LoadValues method
        WeaponManager.Instance.LoadValues();
        Debug.Log("Game values loaded!");
    }
}
