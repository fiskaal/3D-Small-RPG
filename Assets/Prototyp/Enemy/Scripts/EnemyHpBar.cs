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

    private void Start()
    {
        SetHealthBarInvisible();
    }

    public void SetMaxHP(float maxHP)
    {
        slider.maxValue = maxHP; // Set the maximum HP value.
        slider.value = maxHP;    // Set the current HP value to max initially.

        numberMaxHp = maxHP;
        SetHPText();
    }
    
    IEnumerator SmoothSliderValueChange(float targetValue, float duration)
    {
        float elapsedTime = 0f;
        float startValue = slider.value;

        while (elapsedTime < duration)
        {
            slider.value = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            numberCurrentHp = slider.value;
            SetHPText();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final value is set precisely
        slider.value = targetValue;
        numberCurrentHp = targetValue;
        SetHPText();
    }

    public void SetHP(float currentHP)
    {
        /*
        slider.value = currentHP; // Update the current HP value on the UI.

        numberCurrentHp = currentHP;
        SetHPText();
        */
        StartCoroutine(SmoothSliderValueChange(currentHP, 1f));
    }

    public void SetHPText()
    {
        hpText.text = numberCurrentHp.ToString("F0") + "/" + numberMaxHp;
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