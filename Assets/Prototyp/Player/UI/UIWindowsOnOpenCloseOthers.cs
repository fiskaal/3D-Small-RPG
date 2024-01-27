using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class UIWindowsOnOpenCloseOthers : MonoBehaviour
{
    public GameObject[] allUiWindows;

    private void OnEnable()
    {
        for (int i = 0; i < allUiWindows.Length; i++)
        {
            if (allUiWindows[i] != null)
            {
                if (gameObject != allUiWindows[i])
                {
                    allUiWindows[i].SetActive(false);
                }
            }
        }
    }
}
