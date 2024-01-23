using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageReleasePetController : MonoBehaviour
{
    public GameObject cageHighlightObject;
    public GameObject eButton;
    public bool petReleased = false;
    public bool keyGotten = false;

    private Collider playerCollider;

    // Define the specific 3D collider
    public Collider interactionCollider;

    private void Start()
    {
 
    }

    private void Update()
    {
        // Check if the player is within the interaction range and presses the 'E' key
        if (Input.GetKeyDown(KeyCode.E) && !petReleased && keyGotten && IsPlayerInRange())
        {
            ReleasePet();
        }

        if (petReleased == false)
        {
            if (keyGotten)
            {
                if (IsPlayerInRange())
                {
                    eButton.SetActive(true);
                }
                else
                {
                    eButton.SetActive(false);
                }
            }
        }
        else
        {
            eButton.SetActive(false);
        }
    }

    private bool IsPlayerInRange()
    {
        // Find the player object by tag
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            // Get the player's collider
            playerCollider = playerObject.GetComponent<Collider>();
        }
        else
        {
            Debug.LogError("Player object not found. Make sure it has the 'Player' tag.");
        }
        // Check if the player is within the range of the collider
        return interactionCollider.bounds.Contains(playerCollider.bounds.center);
    }

    private void ReleasePet()
    {
        // Set petReleased to true when the player releases the pet
        petReleased = true;
        cageHighlightObject.SetActive(false);
        Debug.Log("Pet released!");
    }

    public void CageToUnlockAnim()
    {
        cageHighlightObject.SetActive(true);
    }
}
