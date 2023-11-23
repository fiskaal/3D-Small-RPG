using UnityEngine;

public class TeleportOnStart : MonoBehaviour
{
    public Transform teleportDestination; // The destination where the player should be teleported

    void Start()
    {
        TeleportPlayer();
    }

    void TeleportPlayer()
    {
        if (teleportDestination != null)
        {
            // Teleport the player to the specified destination
            // You can adjust the destination position based on your game's requirements
            // For example, you might want to add an offset to the destination position
            // based on the size of the player GameObject.
            PlayerTeleport(teleportDestination.position);
        }
        else
        {
            Debug.LogWarning("Teleport destination not set!");
        }
    }

    void PlayerTeleport(Vector3 destination)
    {
        // Find the player GameObject by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Teleport the player to the specified destination
            player.transform.position = destination;
        }
        else
        {
            Debug.LogError("Player not found for teleportation!");
        }
    }
}
