using UnityEngine;
using TMPro;

public class ManagerPickups : MonoBehaviour
{
    public static int wood = 0; // Number of wood collected
    public static int stone = 0; // Number of stones collected
    public static int iron = 0; // Number of iron collected
    public static int soul = 250; // Number of souls collected

    public TextMeshProUGUI woodText; // Reference to the UI TextMeshProUGUI component displaying wood count
    public TextMeshProUGUI stoneText; // Reference to the UI TextMeshProUGUI component displaying stone count
    public TextMeshProUGUI ironText; // Reference to the UI TextMeshProUGUI component displaying iron count
    public TextMeshProUGUI soulText; // Reference to the UI TextMeshProUGUI component displaying soul count

    [SerializeField] public Animator _soulAnimator; // Exposed in the Unity Editor

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
        // Update UI TextMeshProUGUI components based on wood, stone, iron, and soul counts
        // woodText.text = wood.ToString();
        // stoneText.text = stone.ToString();
        // ironText.text = iron.ToString();
         soulText.text = soul.ToString();
       // soulText.SetText(soul.ToString());
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
