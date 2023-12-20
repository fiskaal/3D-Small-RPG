using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellCoolDownUi : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private TextMeshProUGUI timer;

    void Update()
    {
        // Update UI
        if (_character != null)
        {
            if (_character.currentSpecialAttackisReady)
            {
                // If dash is ready, hide the cooldown visual
                cooldownImage.fillAmount = 1;
                timer.text = "";
            }
            else
            {
                // Calculate the fill amount based on the remaining cooldown time
                float fillAmount = 0 + (_character.currentSpecialAttackTimePassed / _character.currentSpecialAttackCooldown);
                cooldownImage.fillAmount = fillAmount;

                // Calculate remaining time in seconds
                float remainingTime = _character.currentSpecialAttackCooldown - _character.currentSpecialAttackTimePassed;
                timer.text = Mathf.Ceil(remainingTime).ToString(); // Display the remaining time as text
            }
        }
        else
        {
            Debug.LogWarning("Please assign the Character script reference in the inspector.");
        }

        if (cooldownImage == null)
        {
            Debug.LogWarning("Please assign a UI Image for cooldown visualization in the inspector.");
        }

        if (timer == null)
        {
            Debug.LogWarning("Please assign a TextMeshProUGUI for timer display in the inspector.");
        }
    }
}