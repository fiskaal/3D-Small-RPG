using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutTextOnEnable : MonoBehaviour
{
    private Text textComponent;
    private bool fading = false;

    void Start()
    {
        textComponent = GetComponent<Text>();
        if (textComponent == null)
        {
            Debug.LogError("Text component not found on the GameObject.");
            return;
        }
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 1f);

        // Start fading out immediately when the object is spawned
        if (textComponent != null)
        {
            fading = true;
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        float fadeDuration = 4f; // 1 second duration

        while (elapsedTime < fadeDuration)
        {
            if (textComponent != null)
            {
                Color color = textComponent.color;
                textComponent.color = new Color(color.r, color.g, color.b, Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration));
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (textComponent != null)
        {
            Color color = textComponent.color;
            textComponent.color = new Color(color.r, color.g, color.b, 0f); // Ensure alpha is exactly 0 at the end of the fading
        }

        fading = false;
    }
}
