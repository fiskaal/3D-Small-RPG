using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEnchantMaker : MonoBehaviour
{
   [SerializeField] private GameObject FireHolder;
   [SerializeField] private GameObject FireEnchantPreFab;
   private bool fireEnchant;
   
   [SerializeField] private GameObject LightningHolder;
   [SerializeField] private GameObject LightningEnchantPreFab;
   private bool lightningEnchant = false;


   private float previousWeaponDamage;
   
   private DamageOfEverything _damageOfEverything;
   private void Start()
   {
      _damageOfEverything = gameObject.GetComponentInParent<DamageOfEverything>();

      EnchanteLightning();
   }

   private void EnchanteFire()
   {
      
      if (!fireEnchant)
      {
         GameObject fire = Instantiate(FireEnchantPreFab, FireHolder.transform);

         previousWeaponDamage = _damageOfEverything.weaponDamage;
         _damageOfEverything.weaponDamage = _damageOfEverything.weaponDamage + _damageOfEverything.fireEnchantDamageBonus;
         fireEnchant = true;
      }
      else
      {
         KeepWeaponEnchanted();
      }
   }

   private void UnEnchantFire()
   {
      Destroy(FireEnchantPreFab);
      _damageOfEverything.weaponDamage = previousWeaponDamage;
      previousWeaponDamage = 0f;

      fireEnchant = false;
   }
   
   private void EnchanteLightning()
   {
      if (!lightningEnchant)
      {
         GameObject lightning = Instantiate(LightningEnchantPreFab, FireHolder.transform);

         previousWeaponDamage = _damageOfEverything.weaponDamage;
         _damageOfEverything.weaponDamage = _damageOfEverything.weaponDamage + _damageOfEverything.lightningEnchantDamageBonus;
         lightningEnchant = true;
      }
      else
      {
         KeepWeaponEnchanted();
      }
   }
   
   private void UnEnchantLightning()
   {
      Destroy(LightningEnchantPreFab);
      _damageOfEverything.weaponDamage = previousWeaponDamage;
      previousWeaponDamage = 0f;

      lightningEnchant = false;
   }



   private void KeepWeaponEnchanted()
   {
      if (fireEnchant)
      {
         Instantiate(FireEnchantPreFab, FireHolder.transform);
      }
      
      if (lightningEnchant)
      {
         Instantiate(LightningEnchantPreFab, FireHolder.transform);
      }
   }
}
