using UnityEngine;
using UnityEngine.UI;

public class StatsUpgradeButton : MonoBehaviour
{
    public DamageOfEverything damageScript;
    public HealthSystem playerHealthScript;

    public float maxHealthIncreaseAmount = 10f; // Change the variable name
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
        // Increase maxHealth instead of health
        playerHealthScript.maxHealth += maxHealthIncreaseAmount;

        // Add the increased maxHealth to the actual health
        playerHealthScript.health += maxHealthIncreaseAmount;

        // Ensure that health remains within the valid range [0, maxHealth]
        playerHealthScript.health = Mathf.Clamp(playerHealthScript.health, 0f, playerHealthScript.maxHealth);

        // Increase other damage stats
        damageScript.weaponDamage += weaponDamageIncreaseAmount;
        damageScript.lightingStrikeDamage += lightningStrikeDamageIncreaseAmount;
        damageScript.fireEnchantDamageBonus += fireEnchantDamageBonusIncreaseAmount;
        damageScript.lightningEnchantDamageBonus += lightningEnchantDamageBonusIncreaseAmount;
        damageScript.enchantedWeaponDamage += enchantedWeaponDamageIncreaseAmount;
    }
}
