using UnityEngine;
using UnityEngine.AI;

public class Chest : MonoBehaviour
{
    [Header("Chest Settings")]
    public GameObject SpriteE;
    public Animation chestAnimation; // Reference to the Animation component of the chest
    public ParticleSystem particleSystemObject; // Reference to the Particle System component
    public KeyCode interactKey = KeyCode.E;
    public Collider interactionCollider;
    private bool isOpen = false;
    private bool chestLooted = false;
    private Collider playerCollider;

    public GameObject[] lootItems; // Array of loot items to spawn
    public Vector2[] lootQuantities; // Array of loot quantities as a range (min, max)

    // Set the chest state (isOpen)
    public void SetChestState(bool state)
    {
        isOpen = state;

        // Activate the particle system if isOpen is true
        if (isOpen && particleSystemObject != null)
        {
            particleSystemObject.gameObject.SetActive(true);
        }

    }

    // Drop loot when the chest is looted
    private void DropLoot()
    {
        // Check if the chest has already been looted
        if (chestLooted)
        {
            return; // Exit the function if already looted
        }

        for (int i = 0; i < lootItems.Length; i++)
        {
            int quantity = Random.Range((int)lootQuantities[i].x, (int)lootQuantities[i].y + 1);

            for (int j = 0; j < quantity; j++)
            {
                Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(1f, 3f);
                Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);

                // Use NavMesh.SamplePosition to find a valid position on the NavMesh
                if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
                {
                    spawnPosition = hit.position + Vector3.up;
                }

                Instantiate(lootItems[i], spawnPosition, Quaternion.identity);
                particleSystemObject.gameObject.SetActive(false);
            }
        }

        // Mark the chest as looted
        chestLooted = true;


    }

    private void Update()
    {
        // Check if the player is pressing the "E" key, isOpen is true, and the chest is not looted
        if (isOpen && Input.GetKeyDown(interactKey) && !chestLooted && IsPlayerInRange())
        {
            // Trigger the chest opening animation
            if (chestAnimation != null)
            {
                chestAnimation.Play(); // Assumes the default animation clip is for opening
            }

            // The chest is looted when the player presses "E"
            // Drop loot when the chest is looted
            DropLoot();
        }

        if (chestLooted == false)
        {
            if (isOpen)
            {
                if (IsPlayerInRange())
                {
                    SpriteE.SetActive(true);
                }
                else
                {
                    SpriteE.SetActive(false);
                }
            }
        }
        else
        {
            SpriteE.SetActive(false);
        }
    }

    private bool IsPlayerInRange()
    {
        // Find the player object by tag
        HealthSystem playerObject = FindObjectOfType<HealthSystem>();

        if (playerObject != null)
        {
            // Get the player's collider
            playerCollider = playerObject.GetComponent<Collider>();
        }
        else
        {
            Debug.LogError("Player object not found. Make sure it has the 'Player' tag.");
        }
        // Check if the player is within the range of the collider
        return interactionCollider.bounds.Contains(playerCollider.bounds.center);
    }
}
