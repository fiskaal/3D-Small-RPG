using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTimedDeath : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // Increment the timer by the time that has passed since the last frame.

        if (timer >= destroyTime)
        {
            Destroy(gameObject); // Destroy the game object this script is attached to.
        }
    }
}
