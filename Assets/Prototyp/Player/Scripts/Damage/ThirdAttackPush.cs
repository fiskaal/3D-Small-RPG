using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdAttackPush : MonoBehaviour
{
    public Transform playerTransform;  // Reference to the player's transform.
    public LayerMask enemyLayer;      // Layer mask for the enemy layer.

    public float pushForce = 10f;     // The force with which enemies are pushed away from the player.
    public float pushRadius = 10f;    // The radius within which enemies are affected.
    public float pushDuration = 1f;   // The duration of the pushing effect.

    private void Start()
    {
        playerTransform = GetComponentInParent<Transform>();
    }

    public void PushEnemiesAway()
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, pushRadius, enemyLayer);

        foreach (Collider col in hitColliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Calculate the direction away from the player.
                Vector3 pushDirection = col.transform.position - playerTransform.position;
                pushDirection.y = 0f;  // Ensure the force is applied horizontally.

                // Normalize the direction and apply force to push the enemy away.
                pushDirection.Normalize();
                rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }

        // You can also add a coroutine to reset the enemy's physics after a duration.
        StartCoroutine(ResetEnemyPhysics(hitColliders));
    }

    private IEnumerator ResetEnemyPhysics(Collider[] enemies)
    {
        yield return new WaitForSeconds(pushDuration);

        foreach (Collider col in enemies)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Reset the enemy's physics state.
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}