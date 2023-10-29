using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public GameObject[] objectsToActivate; // Array of game objects to activate
    public GameObject[] objectsToDeactivate; // Array of game objects to deactivate
    public GameObject warningMessage; // Reference to the warning game object
    public int woodCost = 5; // Wood cost required for activation
    public int stoneCost = 3; // Stone cost required for activation
    public int ironCost = 2; // Iron cost required for activation

    public void OnButtonClick()
    {
        // Check if the player has enough resources to activate the objects
        if (ManagerPickups.wood >= woodCost && ManagerPickups.stone >= stoneCost && ManagerPickups.iron >= ironCost)
        {
            // Deduct the cost from the resources
            ManagerPickups.UpdateWoodCount(-woodCost);
            ManagerPickups.UpdateStoneCount(-stoneCost);
            ManagerPickups.UpdateIronCount(-ironCost);

            // Activate the game objects in the objectsToActivate array
            foreach (GameObject obj in objectsToActivate)
            {
                obj.SetActive(true);
            }

            // Deactivate the game objects in the objectsToDeactivate array
            foreach (GameObject obj in objectsToDeactivate)
            {
                obj.SetActive(false);
            }

            // Deactivate the warning message if it was previously active
            if (warningMessage.activeSelf)
            {
                warningMessage.SetActive(false);
            }
        }
        else
        {
            // Not enough resources, activate the warning message
            warningMessage.SetActive(true);
        }
    }
}
