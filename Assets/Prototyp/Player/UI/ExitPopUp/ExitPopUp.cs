using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPopUp : MonoBehaviour
{
    public GameObject exitPopUp;
    private bool isPopupVisible = false;

    void Start()
    {
        HidePopup(); // Ensure the popup is hidden initially
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPopupVisible)
            {
                HidePopup();
            }
            else
            {
                ShowPopup();
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