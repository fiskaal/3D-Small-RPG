using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{

    public float volumeSteps = 0.2f;
    public float volumeHit = 0.2f;
    public float volumeAttack = 1f;


    public AudioSource audioSource;

    public AudioClip hit;
    public AudioClip attackWhoosh;
    public AudioClip attackWhoosh1;
    public AudioClip[] steps;
    
    

    
    private void PlayFootStep()
    {
        if (audioSource == null)
        { 
            return;
        }
        
        // Check if there is at least one AudioClip in the array
        if (steps.Length > 0)
        {
            // Select a random index from the stoneFootSteps array
            int randomIndex = Random.Range(0, steps.Length);

            // Get the AudioClip at the random index
            AudioClip randomFootstep = steps[randomIndex];

            // Assign the selected AudioClip to the AudioSource
            audioSource.clip = randomFootstep;

            
            audioSource.volume = volumeSteps;

            // Play the assigned AudioClip
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No stone footstep audio clips assigned.");
        }
    }
    
    public void PlayHit()
    {
        if (audioSource == null)
        { 
            return;
        }
        audioSource.clip = hit;
        audioSource.volume = volumeHit;
        audioSource.Play();
    }
    
    public void PlayAttack()
    {
        if (audioSource == null)
        { 
            return;
        }
        audioSource.clip = attackWhoosh;
        audioSource.volume = volumeAttack;
        audioSource.Play();
    }
    
    public void PlayAttack1()
    {
        if (audioSource == null)
        { 
            return;
        }
        audioSource.clip = attackWhoosh1;
        audioSource.volume = volumeAttack;
        audioSource.Play();
    }
}
