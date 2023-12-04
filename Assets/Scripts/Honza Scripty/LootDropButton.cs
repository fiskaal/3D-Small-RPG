using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class LootDropButton : MonoBehaviour
{
    public Button dropLootButton; // Reference to your UI button
    public GameObject[] lootItems; // Reference to your loot items
    public Vector2[] lootQuantities; // Reference to your loot quantities

    private Transform playerTransform; // Reference to the player's transform

    private void Start()
    {
        // Find the player GameObject by tag and get its transform
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found. Make sure to set the Player tag on the player GameObject.");
        }

        // Attach the DropLoot method to the button's onClick event
        dropLootButton.onClick.AddListener(DropLoot);
    }

    void DropLoot()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player transform is null. Make sure to set the Player tag on the player GameObject.");
            return;
        }

        for (int i = 0; i < lootItems.Length; i++)
        {
            int quantity = Random.Range((int)lootQuantities[i].x, (int)lootQuantities[i].y + 1);

            for (int j = 0; j < quantity; j++)
            {
                Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(1f, 3f);
                Vector3 spawnPosition = playerTransform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);

                // Use NavMesh.SamplePosition to find a valid position on the NavMesh
                if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
                {
                    spawnPosition = hit.position + Vector3.up;
                }

                Instantiate(lootItems[i], spawnPosition, Quaternion.identity);
            }
        }
    }
}
