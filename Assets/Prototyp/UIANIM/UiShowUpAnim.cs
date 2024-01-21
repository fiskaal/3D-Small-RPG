using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UiShowUpAnim : MonoBehaviour
{
    public AnimationCurve scaleCurve;
    public float animationSpeed = 3f; // You can adjust this speed factor

    private Vector3 desiredScale;

    private void Awake()
    {
        desiredScale = gameObject.transform.localScale;
    }

    private void OnEnable()
    {
        StartCoroutine(ScaleOverTime());
    }

    IEnumerator ScaleOverTime()
    {
        float timer = 0f;

        while (timer <= 1f)
        {
            float curveTime = scaleCurve.Evaluate(timer);
            transform.localScale = desiredScale * curveTime;

            timer += Time.unscaledDeltaTime * animationSpeed / scaleCurve.length; // Adjusting the timer based on speed and curve length
            yield return null;
        }

        transform.localScale = desiredScale; // Ensure the final scale is precise
    }
}
