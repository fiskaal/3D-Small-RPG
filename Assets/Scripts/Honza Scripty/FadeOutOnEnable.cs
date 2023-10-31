using UnityEngine;
using System.Collections;

public class FadeOutOnEnable : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private bool fading = false;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1f;

        // Start fading out immediately when the object is spawned
        if (canvasGroup != null)
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
            if (canvasGroup != null) // Check if canvasGroup is not null before accessing it
            {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f; // Ensure alpha is exactly 0 at the end of the fading
        }

        fading = false;
    }
}
