using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Cinemachine;

public class EnemyTarget : MonoBehaviour
{
    [FormerlySerializedAs("player")] public Transform cameraTransform;
    public float viewConeAngle = 45f;
    public float maxViewDistance = 10f;

    public GameObject currentTarget;
    private GameObject lastTarget;
    
    public CinemachineVirtualCamera virtualCamera;


    private void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

    }

    void Update()
    {
        DetectEnemiesInCone();
    }

    void DetectEnemiesInCone()
    {
        Collider[] hitColliders = Physics.OverlapSphere(cameraTransform.position, maxViewDistance, LayerMask.GetMask("Enemy"));

        float minAngleDifference = float.MaxValue;
        GameObject closestEnemy = null;

        foreach (var collider in hitColliders)
        {
            Vector3 directionToEnemy = (collider.transform.position - cameraTransform.position).normalized;
            float angleToEnemy = Vector3.Angle(cameraTransform.forward, directionToEnemy);

            if (angleToEnemy <= viewConeAngle)
            {
                float angleDifference = Mathf.Abs(angleToEnemy);
                if (angleDifference < minAngleDifference)
                {
                    minAngleDifference = angleDifference;
                    closestEnemy = collider.gameObject;
                }
            }
        }

        if (closestEnemy != null)
        {
            currentTarget = closestEnemy;

            if (currentTarget != lastTarget)
            {
                if (currentTarget != null)
                {
                    currentTarget.GetComponentInChildren<Outline>().SetOutlineWidth(5);
                    currentTarget.GetComponentInChildren<EnemyHpBar>().SetHealthBarVisible();
                    
                    //virtualCamera.Follow = currentTarget.transform;
                    //virtualCamera.LookAt = currentTarget.transform;
                }

                if (lastTarget != null)
                {
                    lastTarget.GetComponentInChildren<Outline>().SetOutlineWidth(0);
                    lastTarget.GetComponentInChildren<EnemyHpBar>().SetHealthBarInvisible();
                }
            }
            
            lastTarget = closestEnemy;
        }
        else
        {
            // No valid enemy in view cone
            currentTarget = null;
            
            if (lastTarget != null)
            {
                lastTarget.GetComponentInChildren<Outline>().SetOutlineWidth(0);
                lastTarget.GetComponentInChildren<EnemyHpBar>().SetHealthBarInvisible();
            }

            lastTarget = null;
        }
    }
}