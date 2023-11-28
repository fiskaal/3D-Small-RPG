using UnityEngine;
using UnityEngine.UI;

public class ManagerPickups : MonoBehaviour
{
    public static int wood = 0; // Number of wood collected
    public static int stone = 0; // Number of stones collected
    public static int iron = 0; // Number of iron collected
    public static int soul = 0; // Number of souls collected

    public Text woodText; // Reference to the UI Text component displaying wood count
    public Text stoneText; // Reference to the UI Text component displaying stone count
    public Text ironText; // Reference to the UI Text component displaying iron count
    public Text soulText; // Reference to the UI Text component displaying soul count

    private static ManagerPickups instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Update UI Text components based on wood, stone, iron, and soul counts
        woodText.text = wood.ToString();
        stoneText.text = stone.ToString();
        ironText.text = iron.ToString();
        soulText.text = soul.ToString();
    }

    // Methods to update wood, stone, iron, and soul counts
    public static void UpdateWoodCount(int amount)
    {
        wood += amount;
    }

    public static void UpdateStoneCount(int amount)
    {
        stone += amount;
    }

    public static void UpdateIronCount(int amount)
    {
        iron += amount;
    }

    public static void UpdateSoulCount(int amount)
    {
        soul += amount;
    }
}
