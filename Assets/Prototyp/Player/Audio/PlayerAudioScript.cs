using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAudioScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] stoneFootSteps;

    public AudioClip jump;
    public AudioClip fallImpact;
    public AudioClip dash;

    public AudioClip[] hitAu;
    public AudioClip blockHit;
    [FormerlySerializedAs("blocBreak")] public AudioClip blockBreak;

    public AudioClip[] swordDraw;
    
    public AudioClip slash0;
    public AudioClip slash1;
    public AudioClip slash2;
    public AudioClip slash2Impact;

    public AudioClip pickUp;


    public void PlayPickUp()
    {
        audioSource.clip = pickUp;
        audioSource.volume = 0.2f;
        audioSource.Play();
    }
    
    public void PlayFootStep()
    {
        PlayRandomStoneFootStep();
        audioSource.volume = 0.4f;
    }

    public void PlayHit()
    {
        // Check if there is at least one AudioClip in the array
        if (hitAu.Length > 0)
        {
            // Select a random index from the stoneFootSteps array
            int randomIndex = Random.Range(0, hitAu.Length);

            // Get the AudioClip at the random index
            AudioClip randomAu = hitAu[randomIndex];

            // Assign the selected AudioClip to the AudioSource
            audioSource.clip = randomAu;

            audioSource.volume = 0.4f;
            // Play the assigned AudioClip
            audioSource.Play();
        }
    }
    
    public void PlayBlock()
    {
        audioSource.clip = blockHit;
        audioSource.volume = 0.4f;
        audioSource.Play();
    }
    
    public void PlayBlockBreak()
    {
        audioSource.clip = blockBreak;
        audioSource.volume = 0.4f;
        audioSource.Play();
    }
    
    public void PlayShieldBreak()
    {
        //audioSource.clip = hitAu;
        audioSource.volume = 0.4f;
        audioSource.Play();
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

    public void PlayDash()
    {
        audioSource.clip = dash;
        audioSource.volume = 0.4f;
        audioSource.Play();
    }

    public void PlaySlash0()
    {
        audioSource.clip = slash0;
        audioSource.volume = 0.4f;
        audioSource.Play();
    }
    
    public void PlaySlash1()
    {
        audioSource.clip = slash1;
        audioSource.volume = 0.4f;
        audioSource.Play();
    }
    
    public void PlaySlash2()
    {
        audioSource.clip = slash2;
        audioSource.volume = 0.4f;
        audioSource.Play();
    }
    
    public void PlaySlash2Impact()
    {
        audioSource.clip = slash2Impact;
        audioSource.volume = 0.4f;
        audioSource.Play();
    }
    
    public void PlaySwordDraw()
    {
        // Check if there is at least one AudioClip in the array
        if (swordDraw.Length > 0)
        {
            // Select a random index from the stoneFootSteps array
            int randomIndex = Random.Range(0, swordDraw.Length);

            // Get the AudioClip at the random index
            AudioClip randomAu = swordDraw[randomIndex];

            // Assign the selected AudioClip to the AudioSource
            audioSource.clip = randomAu;

            audioSource.volume = 0.4f;
            // Play the assigned AudioClip
            audioSource.Play();
        }
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
