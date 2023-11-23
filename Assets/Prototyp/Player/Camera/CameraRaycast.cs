
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    public bool rayCastingIsActive;
    
    [SerializeField]private Camera mainCamera;
    private RaycastHit hitInfo;

    private Outline currentEnemyOutline;
    private Outline lastEnemyOutline;

    public GameObject currentEnemy;

    [SerializeField] private float raycastSize = 0.2f; // Adjust the size of the raycast area
    [SerializeField] private float raycastLength = 100f; // Adjust the length of the raycast

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (rayCastingIsActive)
        {
            // Cast a ray within the specified area of the screen to detect enemies
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Center of the screen
            Rect screenRect = new Rect(0.5f - raycastSize / 2, 0.5f - raycastSize / 2, raycastSize, raycastSize);

            if (Physics.Raycast(ray, out hitInfo, raycastLength))
            {
                if (hitInfo.collider.CompareTag("Enemy"))
                {
                    // Get the OutlineScript component from the hit object
                    currentEnemyOutline = hitInfo.collider.GetComponentInChildren<Outline>();

                    if (lastEnemyOutline != null && lastEnemyOutline != currentEnemyOutline)
                    {
                        lastEnemyOutline.SetOutlineWidth(0f);
                        currentEnemy = lastEnemyOutline.gameObject;
                    }

                    // Check if the enemy has an OutlineScript attached
                    if (currentEnemyOutline != null)
                    {
                        currentEnemyOutline.SetOutlineWidth(10.0f); // Change the outline width
                        lastEnemyOutline = currentEnemyOutline;
                        currentEnemy = currentEnemyOutline.gameObject;
                    }
                }
                else
                {
                    // No enemy detected, reset outline width
                    if (lastEnemyOutline != null)
                    {
                        lastEnemyOutline.SetOutlineWidth(0f); // Reset outline width to default
                        currentEnemy = null;
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // Visualize the raycast in the Scene view
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Center of the screen
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(ray.origin, ray.direction * raycastLength);

    }
}
