using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplingVFX : MonoBehaviour
{
    public GameObject fromGroundJumpVFX;

    public void PlayFromGroundJumpVFX()
    {
        fromGroundJumpVFX.SetActive(true);
        StartCoroutine(DeactivateVFXAfterDuration());
    }

    IEnumerator DeactivateVFXAfterDuration()
    {
        yield return new WaitForSeconds(1f);
        fromGroundJumpVFX.SetActive(false);
    }
}