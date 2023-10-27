using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] float shieldDurability = 3; // Change the value as needed

    private Animation shieldAnimation;
    private bool destroyShield;
    [SerializeField] private float destroyShieldAnimTime;
    [SerializeField] private float timePassed;
    private void Start()
    {
        shieldAnimation = gameObject.GetComponent<Animation>();
        destroyShield = false;
        timePassed = 0f;
    }

    public void TakeDamage(float damageAmount)
    {
        shieldDurability -= damageAmount;
        shieldAnimation.Play("ShieldDamage");
        if (shieldDurability <= 0)
        {
            BreakShield();
        }
    }

    void BreakShield()
    {
        shieldAnimation.Play("ShieldDestroy"); // Play a visual effect
        destroyShield = true;
    }

    private void Update()
    {
        if (destroyShield)
        {
            if (destroyShieldAnimTime <= timePassed)
            {
                Destroy(gameObject);
            }

            timePassed += Time.deltaTime;
        }
    }
}
