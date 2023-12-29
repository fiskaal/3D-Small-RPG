using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LootDropButton : MonoBehaviour
{
    public string playerTag = "Player"; // Tag of the player GameObject
    public GameObject[] lootItems; // Array of loot item prefabs
    public Vector2[] lootQuantities; // Array of loot quantity ranges
    public Button lootButton; // Reference to the UI button

    public float minSpawnDistance = 5f; // Minimum distance for loot spawn
    public float maxSpawnDistance = 10f; // Maximum distance for loot spawn

    private GameObject player; // Reference to the player GameObject

    void Start()
    {
        // Find the player GameObject using the specified tag
        player = GameObject.FindGameObjectWithTag(playerTag);

        if (player == null)
        {
            Debug.LogError("Player not found with tag: " + playerTag);
        }

        if (lootButton == null)
        {
            Debug.LogError("UI button reference not set in LootDrop script!");
        }
        else
        {
            // Attach the LootButtonClicked method to the button's click event
            lootButton.onClick.AddListener(LootButtonClicked);
        }
    }

    void DropLoot()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is null. Make sure the player is tagged with: " + playerTag);
            return;
        }

        for (int i = 0; i < lootItems.Length; i++)
        {
            int quantity = Random.Range((int)lootQuantities[i].x, (int)lootQuantities[i].y + 1);

            for (int j = 0; j < quantity; j++)
            {
                float randomAngle = Random.Range(0f, 2f * Mathf.PI);
                float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

                float xOffset = spawnDistance * Mathf.Cos(randomAngle);
                float zOffset = spawnDistance * Mathf.Sin(randomAngle);

                Vector3 spawnPosition = player.transform.position + new Vector3(xOffset, 0f, zOffset);

                if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
                {
                    spawnPosition = hit.position + Vector3.up;
                }

                Instantiate(lootItems[i], spawnPosition, Quaternion.identity);
            }
        }
    }

    // This method is called when the UI button is clicked
    void LootButtonClicked()
    {
        DropLoot();
    }

    void Update()
    {
        // Optional: You can still use other input methods here if needed
    }
}
