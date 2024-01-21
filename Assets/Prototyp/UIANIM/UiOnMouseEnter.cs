using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiOnMouseEnter : MonoBehaviour
{
    public AnimationCurve scaleCurve;
    public float animationSpeed = 3f;
    public float delay;
    private Vector3 desiredScale;
    private Vector3 scale;
    private Coroutine smoothChangeCoroutine;
    private Vector3 currentScale;

    private void Awake()
    {

        // Add EventTrigger component if not already present
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }

        // Add PointerEnter and PointerExit events
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { OnPointEnter(); });
        eventTrigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { OnPointExit(); });
        eventTrigger.triggers.Add(entryExit);
    }

    public void Start()
    {
        desiredScale = Vector3.one *1.1f;
        scale = Vector3.one;
    }

    public void OnPointEnter()
    {
        if (smoothChangeCoroutine != null)
        {
            StopCoroutine(smoothChangeCoroutine);
        }

        currentScale = transform.localScale;
        smoothChangeCoroutine = StartCoroutine(ScaleOverTime(desiredScale));
    }

    public void OnPointExit()
    {
        if (smoothChangeCoroutine != null)
        {
            StopCoroutine(smoothChangeCoroutine);
            currentScale = transform.localScale;
        }

        currentScale = transform.localScale;
        smoothChangeCoroutine = StartCoroutine(ScaleOverTime(scale));
    }

    IEnumerator ScaleOverTime(Vector3 targetScale)
    {
        yield return new WaitForSeconds(delay);

        float timer = 0f;

        while (timer <= 1f)
        {
            float curveTime = scaleCurve.Evaluate(timer);
            transform.localScale = Vector3.Lerp(currentScale, targetScale, curveTime);

            timer += Time.unscaledDeltaTime * animationSpeed / scaleCurve.length;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
