using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashVFXSCaleChanger : MonoBehaviour
{
    public GameObject slashHolder;
    
    public GameObject axe;
    public GameObject sword;
    public GameObject longSword;
    public GameObject megaSword;

    public Vector3 axeVFXScale;
    public Vector3 swordVFXScale;
    public Vector3 longSwordVFXScale;
    public Vector3 megaSwordVFXScale;

    
    public void Update()
    {
        if (axe.activeSelf)
        {
            slashHolder.transform.localScale = axeVFXScale;
        }
        
        if (sword.activeSelf)
        {
            slashHolder.transform.localScale = swordVFXScale;
        }
        
        if (longSword.activeSelf)
        {
            slashHolder.transform.localScale = longSwordVFXScale;
        }
        
        if (megaSword.activeSelf)
        {
            slashHolder.transform.localScale = megaSwordVFXScale;
        }
    }
}
