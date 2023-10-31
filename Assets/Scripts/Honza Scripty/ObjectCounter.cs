using UnityEngine;
using UnityEngine.UI;

public class ObjectCounter : MonoBehaviour
{
    public string targetTag = "YourTag"; // Specify your target tag here
    public Text countText;
    public GameObject[] objectsToActivate;
    private bool objectsActivated = false;

    void Update()
    {
        // Find all game objects with the specified tag
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(targetTag);

        // Update the count text directly with the count value
        if (countText != null)
        {
            countText.text = targetObjects.Length.ToString();

            // Check if there are no objects with the specified tag
            if (targetObjects.Length == 0 && !objectsActivated)
            {
                // Activate the array of game objects only once
                foreach (GameObject obj in objectsToActivate)
                {
                    obj.SetActive(true);
                }
                objectsActivated = true;
            }
            else if (targetObjects.Length > 0)
            {
                // Reset the flag if objects with the specified tag are present
                objectsActivated = false;
            }
        }
    }
}
