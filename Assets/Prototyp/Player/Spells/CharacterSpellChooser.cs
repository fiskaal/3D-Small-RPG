using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpellChooser : MonoBehaviour
{
    public Character _character;

    [Header("Triggers")] 
    public string fireBall = "spellThrow";

    [Header("CoolDowns")] 
    public float fireballCoolDown = 5f;

    public void Start()
    {
        SetFireBall();
    }

    public void ResetSpecialAction()
    {
        _character.currentSpecialAttackTrigger = null;
        _character.currentSpecialAttackCooldown = 0f;
        
        _character.currentSpecialAttackTimePassed = 0f;
    }
    
    public void SetFireBall()
    {
        _character.currentSpecialAttackTrigger = fireBall;
        _character.currentSpecialAttackCooldown = fireballCoolDown;

        _character.currentSpecialAttackTimePassed = 0f;
    }
}
