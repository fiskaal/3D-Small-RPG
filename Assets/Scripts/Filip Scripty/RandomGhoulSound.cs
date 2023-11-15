using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using UnityEngine;

public class RandomGhoulSound : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component attached to the GameObject
        audioSource = GetComponent<AudioSource>();

        // Start playing sounds with a delay
        StartCoroutine(PlayRandomSoundWithDelay());
    }

    IEnumerator PlayRandomSoundWithDelay()
    {
        while (true)
        {
            // Play a random audio clip from the array
            audioSource.clip = GetRandomClip();
            audioSource.Play();

            // Wait for 5 seconds before playing the next sound
            yield return new WaitForSeconds(5f);
        }
    }

    AudioClip GetRandomClip()
    {
        // Return a random audio clip from the array
        return audioClips[Random.Range(0, audioClips.Length)];
    }
}
