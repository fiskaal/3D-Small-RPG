using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField] GameObject weaponHolder;
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject weaponSheath;


    GameObject currentWeaponInHand;
    GameObject currentWeaponInSheath;
    void Start()
    {
        weaponSheath.SetActive(true);
        weaponHolder.SetActive(false);
        //currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
    }

    public void DrawWeapon()
    {
        weaponSheath.SetActive(false);
        weaponHolder.SetActive(true);
        /*currentWeaponInHand = Instantiate(weapon, weaponHolder.transform);
        Destroy(currentWeaponInSheath);*/
    }

    public void SheathWeapon()
    {
        weaponSheath.SetActive(true);
        weaponHolder.SetActive(false);
        /*currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        Destroy(currentWeaponInHand);*/
    }

    public void StartDealDamage()
    {
        weaponHolder.GetComponentInChildren<DamageDealer>().StartDealDamage();
        //currentWeaponInHand.GetComponentInChildren<DamageDealer>().StartDealDamage();
    }
    public void EndDealDamage()
    {
        weaponHolder.GetComponentInChildren<DamageDealer>().EndDealDamage();
        //currentWeaponInHand.GetComponentInChildren<DamageDealer>().EndDealDamage();
    }
}
