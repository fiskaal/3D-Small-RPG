using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    // Public field to store the name of the scene to be loaded
    public string sceneName;

    // Function to be called when the button is clicked
    public void LoadScene()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }
}
