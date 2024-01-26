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
        StartCoroutine(StartFading());
    }

    private void SetInitialAlpha(Graphic component)
    {
        // Set the initial alpha value to 1
        component.color = new Color(component.color.r, component.color.g, component.color.b, 1f);
    }

    private IEnumerator StartFading()
    {
        // Wait for the specified delay before starting the fading effect
        yield return new WaitForSeconds(delayBeforeFade);

        // Fade the first Image component
        yield return FadeComponent(image1);

        // Fade the second Image component
        yield return FadeComponent(image2);

        // Fade the TextMeshProUGUI component
        yield return FadeComponent(textMeshPro);

        // Deactivate the GameObject after all fading effects are complete
        gameObject.SetActive(false);
    }

    private IEnumerator FadeComponent(Graphic component)
    {
        // Calculate the initial alpha value
        float alpha = component.color.a;

        // Loop until alpha becomes 0
        while (alpha > 0)
        {
            // Reduce alpha gradually over time
            alpha -= Time.deltaTime / fadeDuration;

            // Update the component's color with the new alpha value
            component.color = new Color(component.color.r, component.color.g, component.color.b, alpha);

            // Wait for the next frame
            yield return null;
        }

        // Ensure the alpha is exactly 0
        component.color = new Color(component.color.r, component.color.g, component.color.b, 0);
    }
}
