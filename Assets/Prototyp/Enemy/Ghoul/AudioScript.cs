using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip hit;

    public void PlayHit()
    {
        audioSource.clip = hit;
        audioSource.volume = 0.4f;
        audioSource.Play();
    }
}
