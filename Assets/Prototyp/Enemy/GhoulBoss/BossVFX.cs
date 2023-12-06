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
    
    [SerializeField] private Transform damageDealerTransform;
    [SerializeField] private Transform bossTransform;


    

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
        GameObject VFX = Instantiate(fallAttackVFX, new Vector3(damageDealerTransform.position.x, bossTransform.position.y, damageDealerTransform.position.z), Quaternion.identity);
        VFX.transform.SetParent(null);
    }
    
    public void PreFallAttackPlayVFX()
    {
        GameObject VFXwarning = Instantiate(PreFallAttackVFX, new Vector3(damageDealerTransform.position.x, bossTransform.position.y, damageDealerTransform.position.z), Quaternion.identity);
        VFXwarning.transform.SetParent(null);
    }

}
