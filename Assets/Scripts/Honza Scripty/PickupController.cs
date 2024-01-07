using UnityEngine;

public class PickupController : MonoBehaviour
{
    public string stonePickupTag = "stone pickup"; // Tag for the stone pickups
    public string woodPickupTag = "wood pickup"; // Tag for the wood pickups
    public string ironPickupTag = "iron pickup"; // Tag for the iron pickups
    public string soulPickupTag = "soul pickup"; // Tag for the soul pickups
    private Animator soulAnimator;

    private void Start()
    {
        soulAnimator = FindObjectOfType<ManagerPickups>()._soulAnimator;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(stonePickupTag))
        {
            // Update stone count directly using InventoryManager static variable
            ManagerPickups.stone += 1;
            Destroy(other.gameObject); // Destroy the collected stone object
        }
        else if (other.CompareTag(woodPickupTag))
        {
            // Update wood count directly using InventoryManager static variable
            ManagerPickups.wood += 1;
            Destroy(other.gameObject); // Destroy the collected wood object
        }
        else if (other.CompareTag(ironPickupTag))
        {
            // Increment iron count using InventoryManager static variable
            ManagerPickups.iron += 1;
            Destroy(other.gameObject); // Destroy the collected iron object
        }
        else if (other.CompareTag(soulPickupTag))
        {
            // Increment soul count using InventoryManager static variable
            ManagerPickups.soul += 1;
            Destroy(other.gameObject); // Destroy the collected soul object

              // Trigger animation
            if (soulAnimator != null)
            {
                soulAnimator.Play("TextBounce", 0, 0f);
            }
        }
    }

}
