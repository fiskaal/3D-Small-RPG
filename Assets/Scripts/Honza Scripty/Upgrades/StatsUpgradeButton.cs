using UnityEngine;
using UnityEngine.UI;

public class StatsUpgradeButton : MonoBehaviour
{
    public DamageOfEverything damageScript;
    public HealthSystem playerHealthScript;

    public float healthIncreaseAmount = 10f;
    public float weaponDamageIncreaseAmount = 5f;
    public float lightningStrikeDamageIncreaseAmount = 5f;
    public float fireEnchantDamageBonusIncreaseAmount = 2f;
    public float lightningEnchantDamageBonusIncreaseAmount = 2f;
    public float enchantedWeaponDamageIncreaseAmount = 3f;

    void Start()
    {
        damageScript = GameObject.FindGameObjectWithTag("Player").GetComponent<DamageOfEverything>();
        playerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
    }

    public void IncreaseAllStats()
    {
        playerHealthScript.health += healthIncreaseAmount;
        damageScript.weaponDamage += weaponDamageIncreaseAmount;
        damageScript.lightingStrikeDamage += lightningStrikeDamageIncreaseAmount;
        damageScript.fireEnchantDamageBonus += fireEnchantDamageBonusIncreaseAmount;
        damageScript.lightningEnchantDamageBonus += lightningEnchantDamageBonusIncreaseAmount;
        damageScript.enchantedWeaponDamage += enchantedWeaponDamageIncreaseAmount;
    }
}
