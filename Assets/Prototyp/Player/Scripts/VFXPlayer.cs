using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPlayer : MonoBehaviour
{
    [SerializeField] GameObject VFXSlashHolder;
    [SerializeField] GameObject VFXSlashHolder1;
    [SerializeField] GameObject VFXPrefabSlash1;
    [SerializeField] GameObject VFXPrefabSlash2;

    [SerializeField] private GameObject VFXPrefabSlash3Impact;
    [SerializeField] private GameObject VFXPrefabDash;


    private GameObject currentPlayedVFX;

    public void Start()
    {
        
    }

    public void PlayVFX()
    {
        //currentPlayedVFX = Instantiate(VFXPrefabSlash1, VFXSlashHolder.transform);
        Instantiate(VFXPrefabSlash1, VFXSlashHolder.transform);
        
        //SwitchVFX();
    }

    public void PlayVFX1()
    {
        Instantiate(VFXPrefabSlash2, VFXSlashHolder1.transform);
    }

    public void PlayVFXSlash3Impact()
    {
        Instantiate(VFXPrefabSlash3Impact, gameObject.transform.root);
    }

    public void VFXPlayerDash()
    {
        Instantiate(VFXPrefabDash, gameObject.transform.root);
    }
}
