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
    public float delay;
    private Vector3 desiredScale;

    private bool isAnimating = false;

    private void Awake()
    {
        desiredScale = gameObject.transform.localScale;
    }

    public void Start()
    {
        //transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(ScaleOverTime());
    }

    IEnumerator ScaleOverTime()
    {
        
        yield return new WaitForSeconds(delay);

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

