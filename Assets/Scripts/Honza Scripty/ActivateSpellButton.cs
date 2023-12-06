using UnityEngine;
using UnityEngine.UI;

public class ActivateSpellButton : MonoBehaviour
{
    // Reference to the button component
    private Button button;

    // Soul cost required for activation
    public int soulCost = 10;

    // Reference to the warning message game object
    public GameObject warningMessage;

    // Index of the sword to set the bought status
    public int spellArrayIndex;

    private void Start()
    {
        // Get the Button component on this GameObject
        button = GetComponent<Button>();

        // Attach the method to be called when the button is clicked
        button.onClick.AddListener(UnlockSpell);
    }

    // Method to be called when the button is clicked
    private void UnlockSpell()
    {
        // Check if the player has enough souls to activate
        if (ManagerPickups.soul >= soulCost)
        {
            // Deduct the soul cost from the resources
            ManagerPickups.UpdateSoulCount(-soulCost);

            // Set the spell bought status using the specified index
            SpellManager spellManager = FindObjectOfType<SpellManager>();
            if (spellManager != null)
            {
                // Assuming that SetSpellBoughtStatus is a method in SpellManager
                spellManager.SetSpellBoughtStatus(spellArrayIndex, true);
            }
            else
            {
                Debug.LogError("SpellManager not found in the scene");
            }
        }
        else
        {
            // Not enough souls, activate the warning message
            warningMessage.SetActive(true);
        }
    }
}
