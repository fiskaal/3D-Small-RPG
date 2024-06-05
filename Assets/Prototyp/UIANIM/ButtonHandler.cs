using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button button;
    public float holdDuration = 1.0f;
    public Image fillImage; // Reference to the fill image
    private bool isHolding = false;
    private Coroutine holdCoroutine;

    private void Start()
    {
        // Initially set the button to be non-interactable
        button.interactable = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isHolding)
        {
            // Start holding
            holdCoroutine = StartCoroutine(HoldButton());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isHolding)
        {
            // Stop holding
            StopCoroutine(holdCoroutine);
            isHolding = false;
            fillImage.fillAmount = 0; // Reset fill amount
        }
    }

    IEnumerator HoldButton()
    {
        isHolding = true;
        float holdTimer = 0;

        // Loop until the hold duration is reached
        while (holdTimer < holdDuration)
        {
            holdTimer += Time.unscaledDeltaTime; // Use unscaledDeltaTime to ignore timeScale
            float fillAmount = holdTimer / holdDuration;
            fillImage.fillAmount = fillAmount; // Update fill amount based on hold duration
            yield return null;
        }

        // Make the button interactable and trigger the click event
        button.interactable = true;
        button.onClick.Invoke();
        button.interactable = false; // Reset the button to be non-interactable

        isHolding = false;
        fillImage.fillAmount = 0; // Reset fill amount
    }
}