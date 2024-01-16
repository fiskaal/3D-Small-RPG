using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BossAudioScript : MonoBehaviour
{
    public float volumeSteps = 0.2f;
    public float volumeHit = 0.2f;
    public float volumeAttack = 0.8f;
    public float volumeRoar = 1f;



    public AudioSource audioSource;

    public AudioClip hit;
    public AudioClip handAttackSound;
    public AudioClip handAttackImpactSound;

    public AudioClip legAttackSound;
    public AudioClip legAttackImpactSound;
    
    public AudioClip fallAttackSound;
    public AudioClip fallAttackImpactSound;

    public AudioClip roarSound;
    public AudioClip[] steps;
    
    

    
    private void PlayFootStepAudio()
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
    
    public void PlayHandAttackAudio()
    {
        if (audioSource == null)
        { 
            return;
        }
        audioSource.clip = handAttackSound;
        audioSource.volume = volumeAttack;
        audioSource.Play();
    }
    
    public void PlayHandAttackImpactAudio()
    {
        if (audioSource == null)
        { 
            return;
        }
        audioSource.clip = handAttackImpactSound;
        audioSource.volume = volumeAttack;
        audioSource.Play();
    }
    
    public void PlayLegAttackAudio()
    {
        if (audioSource == null)
        { 
            return;
        }
        audioSource.clip = legAttackSound;
        audioSource.volume = volumeAttack;
        audioSource.Play();
    }
    
    public void PlayLegAttackImpactAudio()
    {
        if (audioSource == null)
        { 
            return;
        }
        audioSource.clip = legAttackImpactSound;
        audioSource.volume = volumeAttack;
        audioSource.Play();
    }
    
    public void PlayFallAttackAudio()
    {
        if (audioSource == null)
        { 
            return;
        }
        audioSource.clip = fallAttackSound;
        audioSource.volume = volumeAttack;
        audioSource.Play();
    }
    
    public void PlayFallAttackImpactAudio()
    {
        if (audioSource == null)
        { 
            return;
        }
        audioSource.clip = fallAttackImpactSound;
        audioSource.volume = volumeAttack;
        audioSource.Play();
    }
    
    public void PlayRoarAudio()
    {
        if (audioSource == null)
        { 
            return;
        }
        audioSource.clip = roarSound;
        audioSource.volume = volumeRoar;
        audioSource.Play();
    }
}
