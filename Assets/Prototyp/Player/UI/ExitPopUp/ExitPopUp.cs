using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExitPopUp : MonoBehaviour
{
    public GameObject exitPopUp;
    private bool isPopupVisible = false;
    public UIWindowsOnOpenCloseOthers exitCloseAllWindowsScript;

    private bool canShow;
    void Start()
    {
        HidePopup(); // Ensure the popup is hidden initially
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)) || (Input.GetKeyDown(KeyCode.Joystick1Button9)))
        {
            for (int i = 0; i < exitCloseAllWindowsScript.allUiWindows.Length; i++)
            {
                if (exitCloseAllWindowsScript.allUiWindows[i].activeSelf == true && exitCloseAllWindowsScript.allUiWindows[i] != exitCloseAllWindowsScript.gameObject)
                {
                    canShow = false;
                    return;
                }
                else
                {
                    canShow = true;
                }
            }

            if (canShow)
            {
                if (exitPopUp.activeSelf == true)
                {
                    HidePopup();
                }
                else
                {
                    ShowPopup();
                }
            }
        }
    }

    void ShowPopup()
    {
        isPopupVisible = true;
        // Add any code here to show the popup (e.g., set the game object active)
        exitPopUp.SetActive(true);
    }

    void HidePopup()
    {
        isPopupVisible = false;
        // Add any code here to hide the popup (e.g., set the game object inactive)
        exitPopUp.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}