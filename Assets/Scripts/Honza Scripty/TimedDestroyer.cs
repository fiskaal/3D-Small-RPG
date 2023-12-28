using UnityEngine;

public class TimedDestroyer : MonoBehaviour
{
    public float destroyTime = 5f; // Adjust this value to set the time before destruction

    void Start()
    {
        // Invoke the DestroyObject method after the specified destroyTime
        Invoke("DestroyObject", destroyTime);
    }

    void DestroyObject()
    {
        // Destroy the specific object this script is attached to
        Destroy(gameObject);
    }
}
