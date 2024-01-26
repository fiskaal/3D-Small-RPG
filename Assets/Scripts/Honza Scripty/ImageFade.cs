using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    public float fadeDuration = 2f; // Time in seconds for fading effect
    public float delayBeforeFade = 3f; // Time in seconds before the fading starts

    public Image image1;
    public Image image2;
    public TextMeshProUGUI textMeshPro;

    private void Start()
    {
        // Set the initial alpha value to 1 for all components
        SetInitialAlpha(image1);
        SetInitialAlpha(image2);
        SetInitialAlpha(textMeshPro);

        // Start the fading process after the specified delay
        StartCoroutine(FadeAllComponents());
    }

    private void SetInitialAlpha(Graphic component)
    {
        // Set the initial alpha value to 1
        component.color = new Color(component.color.r, component.color.g, component.color.b, 1f);
    }

    private IEnumerator FadeAllComponents()
    {
        // Wait for the specified delay before starting the fading effect
        yield return new WaitForSeconds(delayBeforeFade);

        // Create an array of components to fade
        Graphic[] componentsToFade = { image1, image2, textMeshPro };

        // Get the initial alpha values
        float[] initialAlphas = new float[componentsToFade.Length];
        for (int i = 0; i < componentsToFade.Length; i++)
        {
            initialAlphas[i] = componentsToFade[i].color.a;
        }

        // Loop until alpha becomes 0 for all components
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            // Calculate the normalized time
            float normalizedTime = elapsedTime / fadeDuration;

            // Update alpha for all components
            for (int i = 0; i < componentsToFade.Length; i++)
            {
                componentsToFade[i].color = new Color(
                    componentsToFade[i].color.r,
                    componentsToFade[i].color.g,
                    componentsToFade[i].color.b,
                    Mathf.Lerp(initialAlphas[i], 0f, normalizedTime)
                );
            }

            // Wait for the next frame
            yield return null;

            // Update elapsed time
            elapsedTime += Time.deltaTime;
        }

        // Ensure the alpha is exactly 0 for all components
        for (int i = 0; i < componentsToFade.Length; i++)
        {
            componentsToFade[i].color = new Color(
                componentsToFade[i].color.r,
                componentsToFade[i].color.g,
                componentsToFade[i].color.b,
                0f
            );
        }

        // Deactivate the GameObject after all fading effects are complete
        Destroy(gameObject);
    }
}
