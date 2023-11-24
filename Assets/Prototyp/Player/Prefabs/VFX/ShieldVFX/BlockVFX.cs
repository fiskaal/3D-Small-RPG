using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockVFX : MonoBehaviour
{
    [SerializeField]private Animation _animation;

    public void OnEnable()
    {
        _animation.Play("BlockIn");
    }

    public void Damage()
    {
        _animation.Play("BlockDamage");
    }

    public void DisableAfterAnimation()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(PlayAnimationAndDisable());
        }
    }

    private IEnumerator PlayAnimationAndDisable()
    {
        // Play the animation
        _animation.Play("BlockOut");

        // Wait for the animation length or a specific duration
        yield return new WaitForSeconds(_animation.GetClip("BlockOut").length);

        // Disable the object
        gameObject.SetActive(false);
    }
}

