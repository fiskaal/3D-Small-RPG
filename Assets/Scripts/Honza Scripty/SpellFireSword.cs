using UnityEngine;
using TMPro;

public class SpellFireSword : MonoBehaviour
{
    public enum SpellState
    {
        Idle,
        Casting,
        Cooldown
    }

    // Specify the parent GameObject in the Unity Editor
    public GameObject swordHolder;

    // Specify the key to trigger the search
    public KeyCode activationKey = KeyCode.Alpha3;  // Change to KeyCode.Alpha3 for the number key 3

    // Specify the name of the child to find and activate
    public string TargetEffect = "EnchantFire";

    public DamageOfEverything damageScript;
    public float damageFireBonus;

    // Duration of the effect
    public float effectDuration = 5f;  // 5 seconds as an example, change as needed

    // Cooldown between activations
    public float activationCooldown = 10f;  // 10 seconds as an example, change as needed

    private bool bonusApplied = false;
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

    public AudioClip myMp3Sound;
    public AudioSource audioSource;

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
    }

    void ActivateEffect()
    {
        // Get the active child object
        GameObject activeChild = GetActiveChild(swordHolder);

        // Check if there is an active child
        if (activeChild != null)
        {
            // Debug the name of the active child
            Debug.Log("Active Child Object: " + activeChild.name);

            // Iterate through the active child's children
            foreach (Transform child in activeChild.transform)
            {
                // Check if the child's name matches the targetChildName
                if (child.name == TargetEffect)
                {
                    Debug.Log("Found child with name: " + TargetEffect);

                    // Activate the found child
                    child.gameObject.SetActive(true);

                    audioSource.clip = myMp3Sound;
                    audioSource.Play();

                    // Add the bonus to weaponDamage if not already applied
                    if (!bonusApplied)
                    {
                        damageScript.weaponDamage += damageFireBonus;
                        bonusApplied = true;  // Bonus has been applied
                    }

                    // Set timers
                    effectTimer = effectDuration;
                    cooldownTimer = activationCooldown;

                    // Do something with the found child if needed
                }
            }
        }
        else
        {
            Debug.Log("No active child found.");
        }
    }

    void DeactivateEffect()
    {
        // Reset bonus and timers
        damageScript.weaponDamage -= damageFireBonus;
        bonusApplied = false;
        effectTimer = 0f;

        // Get the active child object
        GameObject activeChild = GetActiveChild(swordHolder);

        // Check if there is an active child
        if (activeChild != null)
        {
            // Iterate through the active child's children
            foreach (Transform child in activeChild.transform)
            {
                // Check if the child's name matches the targetChildName
                if (child.name == TargetEffect)
                {
                    // Deactivate the found child
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    GameObject GetActiveChild(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.activeSelf)
            {
                return child.gameObject;
            }
        }

        return null;
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
