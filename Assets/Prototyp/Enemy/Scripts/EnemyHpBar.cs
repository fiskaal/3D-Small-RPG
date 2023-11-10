using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public Slider slider; // Reference to the HP bar UI element.

    public void SetMaxHP(float maxHP)
    {
        slider.maxValue = maxHP; // Set the maximum HP value.
        slider.value = maxHP;    // Set the current HP value to max initially.
    }

    public void SetHP(float currentHP)
    {
        slider.value = currentHP; // Update the current HP value on the UI.
    }
}