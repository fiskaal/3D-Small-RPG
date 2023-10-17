using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//hodi se primo na particle a instacuje se prefab particlu se scriptem nasobe
public class ParticleEndScript : MonoBehaviour
{
    private ParticleSystem particleSystem;

    void Start()
    {
        // Get the reference to the Particle System component
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // Check if the particle system has finished emitting
        if (!particleSystem.IsAlive())
        {
            // Destroy the GameObject, which contains the particle system
            Destroy(gameObject);
        }
    }
}
