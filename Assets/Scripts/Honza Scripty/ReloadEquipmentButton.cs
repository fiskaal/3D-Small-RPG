using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadEquipmentButton : MonoBehaviour
{
    private void Start()
    {
        // Attach the method to the button's click event
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // Find the WeaponManager in the scene
        WeaponManager weaponManager = FindObjectOfType<WeaponManager>();

        // If the weaponManager is found, call the FindHolders method
        if (weaponManager != null)
        {
            weaponManager.FindHolders();
        }
        else
        {
            Debug.LogError("WeaponManager not found. Make sure the script is attached to a GameObject with WeaponManager.");
        }
    }
}
