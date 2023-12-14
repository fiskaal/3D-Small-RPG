using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public Slider slider; // Reference to the HP bar UI element.
    public TextMeshProUGUI hpText;
    private float numberMaxHp;
    private float numberCurrentHp;
    
    public void SetMaxHP(float maxHP)
    {
        slider.maxValue = maxHP; // Set the maximum HP value.
        slider.value = maxHP;    // Set the current HP value to max initially.

        numberMaxHp = maxHP;
        SetHPText();
    }

    public void SetHP(float currentHP)
    {
        slider.value = currentHP; // Update the current HP value on the UI.

        numberCurrentHp = currentHP;
        SetHPText();
    }

    public void SetHPText()
    {
        hpText.text = numberCurrentHp + "/" + numberMaxHp;

    }
    
    public void SetHealthBarInvisible()
    {
        transform.localScale = Vector3.zero;
    }
    
    public void SetHealthBarVisible()
    {
        transform.localScale = Vector3.one;
    }
}