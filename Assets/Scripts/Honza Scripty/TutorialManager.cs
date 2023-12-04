using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static bool isSmithComplete = false;
    public static bool isWizardComplete = false;

    private static TutorialManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Load the saved state when the TutorialManager is created
        LoadPlayerPrefs();
    }

    // Save the state of the boolean variables to PlayerPrefs
    public static void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("IsSmithComplete", isSmithComplete ? 1 : 0);
        PlayerPrefs.SetInt("IsWizardComplete", isWizardComplete ? 1 : 0);

        // Save the changes
        PlayerPrefs.Save();
    }

    // Load the state of the boolean variables from PlayerPrefs
    private static void LoadPlayerPrefs()
    {
        // If the keys do not exist, PlayerPrefs.GetInt returns 0 (false) by default
        isSmithComplete = PlayerPrefs.GetInt("IsSmithComplete", 0) == 1;
        isWizardComplete = PlayerPrefs.GetInt("IsWizardComplete", 0) == 1;
    }
}
