using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOfEverything : MonoBehaviour
{
    public static DamageOfEverything Instance;

    public float weaponDamage;
    public float knockBackForce;
    public float lightingStrikeDamage;

    public float fireEnchantDamageBonus;
    public float lightningEnchantDamageBonus;
    public float enchantedWeaponDamage;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of DamageOfEverything found. Only one instance allowed.");
            Destroy(gameObject); // Destroy the duplicate instance
        }
    }

    // Other methods and variables...
}
