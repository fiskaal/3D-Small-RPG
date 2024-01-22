using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    // Public field to store the name of the scene to be loaded
    public string sceneName;

    public GameObject LoadingPage;

    // Function to be called when the button is clicked
    public void LoadScene()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevel(string nameOfTheScene)
    {
        StartCoroutine(LoadAsynchro(nameOfTheScene));
        
    }

    IEnumerator LoadAsynchro (string nameOfTheScene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nameOfTheScene);

        while (!operation.isDone)
        {
            Debug.Log(operation.progress);
            LoadingPage.SetActive(true);
            yield return null;
        }

    } 
}
