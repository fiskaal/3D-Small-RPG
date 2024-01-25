using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagePetActivator : MonoBehaviour
{
    // Start is called before the first frame update

    private PetStateController petState;

    public GameObject PetBramburek;

    void Start()
    {
        PetActivation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PetActivation()
    {
        petState = FindObjectOfType<PetStateController>();

        if (petState != null)
        {
            if (petState.petFreed)
            {
                PetBramburek.SetActive(true);
            }
            else
            {
                PetBramburek.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("PetStateController not found");
        }
    }
}
