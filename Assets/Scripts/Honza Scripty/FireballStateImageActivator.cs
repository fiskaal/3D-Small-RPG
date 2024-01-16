using UnityEngine;
using TMPro;

public class FireballStateImageActivator : MonoBehaviour
{
    public enum SpellState
    {
        Idle,
        Casting,
        Cooldown
    }


    // Specify the key to trigger the search
    public KeyCode activationKey = KeyCode.Alpha3;  // Change to KeyCode.Alpha3 for the number key 3

    // Duration of the effect
    public float effectDuration = 5f;  // 5 seconds as an example, change as needed

    // Cooldown between activations
    public float activationCooldown = 10f;  // 10 seconds as an example, change as needed

    private float effectTimer = 0f;
    private float cooldownTimer = 0f;

    // References to game objects for each state
    public GameObject idleGameObject;
    public GameObject castingGameObject;
    public GameObject cooldownGameObject;

    // Reference to TextMeshPro UI for displaying effect duration countdown
    public TextMeshProUGUI effectDurationText;

    // Reference to TextMeshPro UI for displaying cooldown duration countdown
    public TextMeshProUGUI cooldownDurationText;

    private SpellState currentState = SpellState.Idle;

    void Start()
    {
        // Initialize TextMeshPro UI text properties to null
        effectDurationText.text = null;
        cooldownDurationText.text = null;

        // Set initial state game object active
        SetStateGameObjectActive(currentState);
    }

    void Update()
    {
        switch (currentState)
        {
            case SpellState.Idle:
                // Update timers
                cooldownTimer -= Time.deltaTime;

                // Update TextMeshPro UI for cooldown duration countdown
                cooldownDurationText.text = (cooldownTimer > 0f) ? Mathf.Max(0, Mathf.Ceil(cooldownTimer)).ToString() : null;

                // Check if the activation key is released and cooldown has passed
                if (Input.GetKeyUp(activationKey) && cooldownTimer <= 0f)
                {
                    currentState = SpellState.Casting;
                    ActivateEffect();
                }

                // Set state game objects active
                SetStateGameObjectActive(currentState);
                break;

            case SpellState.Casting:
                // Update timers
                effectTimer -= Time.deltaTime;

                // Update TextMeshPro UI for effect duration countdown
                effectDurationText.text = (effectTimer > 0f) ? Mathf.Max(0, Mathf.Ceil(effectTimer)).ToString() : null;

                // Check if the effect is active and duration has passed
                if (effectTimer <= 0f)
                {
                    currentState = SpellState.Cooldown;
                    DeactivateEffect();
                    cooldownTimer = activationCooldown;
                }

                // Set state game objects active
                SetStateGameObjectActive(currentState);
                break;

            case SpellState.Cooldown:
                // Update timers
                cooldownTimer -= Time.deltaTime;

                // Update TextMeshPro UI for cooldown duration countdown
                cooldownDurationText.text = (cooldownTimer > 0f) ? Mathf.Max(0, Mathf.Ceil(cooldownTimer)).ToString() : null;

                // Reset to Idle state when cooldown is finished
                if (cooldownTimer <= 0f)
                {
                    currentState = SpellState.Idle;
                }

                // Set state game objects active
                SetStateGameObjectActive(currentState);
                break;
        }

        effectDurationText.text = null;
    }

    void ActivateEffect()
    {
        // Your activation logic here
        // ...

        // Set timers
        effectTimer = effectDuration;
        cooldownTimer = activationCooldown;
    }

    void DeactivateEffect()
    {
        // Your deactivation logic here
        // ...

        // Reset timers
        effectTimer = 0f;
    }

    void SetStateGameObjectActive(SpellState state)
    {
        idleGameObject.SetActive(false);
        castingGameObject.SetActive(false);
        cooldownGameObject.SetActive(false);

        switch (state)
        {
            case SpellState.Idle:
                idleGameObject.SetActive(true);
                break;

            case SpellState.Casting:
                castingGameObject.SetActive(true);
                break;

            case SpellState.Cooldown:
                cooldownGameObject.SetActive(true);
                break;
        }
    }
}
