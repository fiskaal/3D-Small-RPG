using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVFX : MonoBehaviour
{
    [SerializeField] private GameObject handAttackVFX;
    [SerializeField] private GameObject legAttackVFX;
    [SerializeField] private GameObject prelegAttackVFX;
    [SerializeField] private GameObject fallAttackVFX;
    [SerializeField] private GameObject PreFallAttackVFX;


    

    public void HandAttackPlayVFX()
    {
        Instantiate(handAttackVFX, transform);
    }
    
    public void LegAttackPlayVFX()
    {
        Instantiate(legAttackVFX, transform);
    }
    public void PreLegAttackPlayVFX()
    {
        Instantiate(prelegAttackVFX, transform);
    }
    
    public void FallAttackPlayVFX()
    {
        Instantiate(fallAttackVFX, transform);
    }
    
    public void PreFallAttackPlayVFX()
    {
        Instantiate(PreFallAttackVFX, transform);
    }

}
