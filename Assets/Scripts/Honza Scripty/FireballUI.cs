using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireballUI : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private GameObject cooldownObject;
    [SerializeField] private GameObject activeObject;
    [SerializeField] private GameObject idleObject;
    [SerializeField] private TextMeshProUGUI timer;

    private enum UIState
    {
        Cooldown,
        Active,
        Idle
    }

    private UIState currentState = UIState.Idle;
    private float activeTimer = 0.5f;

    void Update()
    {
        // Update UI state
        UpdateUIState();

        // Update UI based on the current state
        UpdateUI();
    }

    void UpdateUIState()
    {
        if (_character != null)
        {
            if (_character.currentSpecialAttackisReady)
            {
                currentState = UIState.Idle;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1) && currentState == UIState.Idle)
            {
                currentState = UIState.Active;
                activeTimer = 0.5f; // Set the active timer to 0.5 seconds
            }
            else
            {
                currentState = UIState.Cooldown;
            }
        }
    }

    void UpdateUI()
    {
        // Hide all game objects initially
        cooldownObject.SetActive(false);
        activeObject.SetActive(false);
        idleObject.SetActive(false);

        switch (currentState)
        {
            case UIState.Cooldown:
                // Calculate the fill amount based on the remaining cooldown time
                float fillAmount = 0 + (_character.currentSpecialAttackTimePassed / _character.currentSpecialAttackCooldown);
                cooldownObject.GetComponent<Image>().fillAmount = fillAmount;

                // Calculate remaining time in seconds
                float remainingTime = _character.currentSpecialAttackCooldown - _character.currentSpecialAttackTimePassed;
                timer.text = Mathf.Ceil(remainingTime).ToString(); // Display the remaining time as text

                cooldownObject.SetActive(true);
                break;


            case UIState.Active:
                // Show the active state for 0.5 seconds
                activeObject.SetActive(true);
                activeTimer -= Time.deltaTime;
                if (activeTimer <= 0)
                {
                    currentState = UIState.Idle; // Switch to idle after 0.5 seconds
                }
                break;

            case UIState.Idle:
                idleObject.SetActive(true);
                timer.text = null;
                break;
        }

        if (_character == null)
        {
            Debug.LogWarning("Please assign the Character script reference in the inspector.");
        }

        if (cooldownObject == null || activeObject == null || idleObject == null)
        {
            Debug.LogWarning("Please assign UI game objects in the inspector.");
        }

        if (timer == null)
        {
            Debug.LogWarning("Please assign a TextMeshProUGUI for timer display in the inspector.");
        }
    }
}
