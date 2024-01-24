using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetStateController : MonoBehaviour
{
    public static PetStateController instance; // Singleton instance
    public bool petFreed;
    private PetBehaviour petScript;
    private CagePetActivatorController cagePetActivatorScript;

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadPetState(); // Load the pet state when the script starts
        ActivateCorrectCagePet();
    }

    // Method to save the current state of petFreed
    public void SavePetState()
    {
        PlayerPrefs.SetInt("PetFreed", petFreed ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Method to load the value from PlayerPrefs
    public void LoadPetState()
    {
        // Use 0 as the default value if "PetFreed" key doesn't exist
        petFreed = PlayerPrefs.GetInt("PetFreed", 0) == 1;

        petScript = FindObjectOfType<PetBehaviour>();
        if (petScript != null)
        {
            petScript.enabled = petFreed; // Enable/disable PetBehaviour based on petFreed state
        }

    }

    public void SetPetFreedFalse()
    {
        //disable local variable
        petFreed = false;

        //disable PetBehaviour = the actual script on the pet
        petScript = FindObjectOfType<PetBehaviour>();
        if (petScript != null)
        {
            petScript.enabled = false; // disable PetBehaviour 
        }

        SavePetState();
        LoadPetState();
    }

    void ActivateCorrectCagePet()
    {
        cagePetActivatorScript = FindObjectOfType<CagePetActivatorController>();
        if (cagePetActivatorScript != null)
        {
            cagePetActivatorScript.ActivateCorrectCageAndPet();
        }
        else
        {
            Debug.Log("CagePetActivatorController script not found");
        }
    }
}
