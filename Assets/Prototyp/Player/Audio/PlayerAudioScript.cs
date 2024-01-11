using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] stoneFootSteps;

    public AudioClip jump;
    public AudioClip fallImpact;

    public void PlayFootStep()
    {
        PlayRandomStoneFootStep();
        audioSource.volume = 0.4f;
    }

    public void PlayJump()
    {
        audioSource.clip = jump;
        audioSource.volume = 0.4f;
        audioSource.Play();
    }
    
    public void PlayFallImpact()
    {
        audioSource.clip = fallImpact;
        audioSource.volume = 0.2f;
        audioSource.Play();
    }

    private void PlayRandomStoneFootStep()
    {
        // Check if there is at least one AudioClip in the array
        if (stoneFootSteps.Length > 0)
        {
            // Select a random index from the stoneFootSteps array
            int randomIndex = Random.Range(0, stoneFootSteps.Length);

            // Get the AudioClip at the random index
            AudioClip randomFootstep = stoneFootSteps[randomIndex];

            // Assign the selected AudioClip to the AudioSource
            audioSource.clip = randomFootstep;

            // Play the assigned AudioClip
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No stone footstep audio clips assigned.");
        }
    }
}
