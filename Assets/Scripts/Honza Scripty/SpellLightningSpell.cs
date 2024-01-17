using UnityEngine;
using TMPro;

public class SpellLightningSpell : MonoBehaviour
{
    public enum SpellState
    {
        Idle,
        Casting,
        Cooldown
    }

    // Specify the key to trigger the spell
    public KeyCode activationKey = KeyCode.Alpha3;  // Change to KeyCode.Alpha3 for the number key 3

    // Duration of the spell effect
    public float effectDuration = 5f;  // 5 seconds as an example, change as needed

    // Cooldown between spell activations
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

    // New GameObject variable for LightningStrikeDealer
    public GameObject LightningStrikeDealer;

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

                // Deactivate LightningStrikeDealer when not casting
                LightningStrikeDealer.SetActive(false);

                // Check if the activation key is released and cooldown has passed
                if (Input.GetKeyUp(activationKey) && cooldownTimer <= 0f)
                {
                    currentState = SpellState.Casting;
                    // Remove the ActivateEffect() call
                }

                // Set state game objects active
                SetStateGameObjectActive(currentState);
                break;

            case SpellState.Casting:
                // Check if effectTimer needs initialization
                if (effectTimer <= 0f)
                {
                    effectTimer = effectDuration;
                }

                // Update timers
                effectTimer -= Time.deltaTime;

                // Update TextMeshPro UI for effect duration countdown
                effectDurationText.text = (effectTimer > 0f) ? Mathf.Max(0, Mathf.Ceil(effectTimer)).ToString() : null;

                // Activate LightningStrikeDealer when casting
                LightningStrikeDealer.SetActive(true);

                // Check if the effect duration has passed
                if (effectTimer <= 0f)
                {
                    currentState = SpellState.Cooldown;
                    // Remove the DeactivateEffect() call
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

                // Deactivate LightningStrikeDealer when on cooldown
                LightningStrikeDealer.SetActive(false);

                // Reset to Idle state when cooldown is finished
                if (cooldownTimer <= 0f)
                {
                    currentState = SpellState.Idle;
                }

                // Set state game objects active
                SetStateGameObjectActive(currentState);
                break;
        }
    }

    // Remove the ActivateEffect() and DeactivateEffect() methods

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
