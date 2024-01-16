using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDisplayActivator : MonoBehaviour
{
    private WeaponManager weaponManager;

    public GameObject fireballBought;
    public GameObject fireballNotBought;
    void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>();
        if (weaponManager == null)
        {
            Debug.LogWarning("WeaponManager not found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponManager != null)
        {
            if (weaponManager.Fireball)
            {
                // If Fireball is bought, activate the "bought" display and deactivate the "not bought" display
                fireballBought.SetActive(true);
                fireballNotBought.SetActive(false);
            }
            else
            {
                // If Fireball is not bought, activate the "not bought" display and deactivate the "bought" display
                fireballBought.SetActive(false);
                fireballNotBought.SetActive(true);
            }
        }
    }
}
