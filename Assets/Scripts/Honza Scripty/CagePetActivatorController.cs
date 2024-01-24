using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CagePetActivatorController : MonoBehaviour
{
    public GameObject freedPet;
    public GameObject freedCage;

    public GameObject prisonPet;
    public GameObject prisonCage;

    private PetStateController petState;

    public void ActivateCorrectCageAndPet()
    {
        petState = FindObjectOfType<PetStateController>();

        if (petState != null)
        {
            if (petState.petFreed)
            {
                freedPet.SetActive(true);
                freedCage.SetActive(true);
                prisonPet.SetActive(false);
                prisonCage.SetActive(false);
            }
            else
            {
                freedPet.SetActive(false);
                freedCage.SetActive(false);
                prisonPet.SetActive(true);
                prisonCage.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("PetStateController not found");
        }
    }
}
