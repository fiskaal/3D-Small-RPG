using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public GameObject objectToActivate; // Reference to the game object to activate
    public GameObject objectToDeActivate; // Reference to the game object to De-activate
    public GameObject warningMessage; // Reference to the warning game object
    public int woodCost = 5; // Wood cost required for activation
    public int stoneCost = 3; // Stone cost required for activation
    public int ironCost = 2; // Iron cost required for activation

    public void OnButtonClick()
    {
        // Check if the player has enough resources to activate the object
        if (ManagerPickups.wood >= woodCost && ManagerPickups.stone >= stoneCost && ManagerPickups.iron >= ironCost)
        {
            // Deduct the cost from the resources
            ManagerPickups.UpdateWoodCount(-woodCost);
            ManagerPickups.UpdateStoneCount(-stoneCost);
            ManagerPickups.UpdateIronCount(-ironCost);

            // Activate the game object
            objectToActivate.SetActive(true);
            // Deactivate Other game object
            objectToDeActivate.SetActive(false);

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