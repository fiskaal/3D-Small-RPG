using System.Collections;
using UnityEngine;

public class WanderingGhost : MonoBehaviour
{
    public float minSpeed = 1f;
    public float maxSpeed = 4f;
    public Collider patrolArea;
    public float curveIntensity = 0.5f;
    public float noiseIntensity = 1f;

    private void Start()
    {
        if (patrolArea == null)
        {
            Debug.LogError("WanderingGhost: Patrol area collider not assigned.");
            enabled = false;
            return;
        }

        StartCoroutine(Wander());
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            float speed = Random.Range(minSpeed, maxSpeed);
            Vector3 randomPoint = GetRandomPointInCollider(patrolArea);

            yield return StartCoroutine(MoveTo(randomPoint, speed));
        }
    }

    private IEnumerator MoveTo(Vector3 targetPosition, float speed)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < 1f) // Continue until reaching the target position
        {
            float curveFactor = Mathf.SmoothStep(0f, 1f, elapsedTime);
            float noiseFactor = Mathf.PerlinNoise(Time.time * noiseIntensity, 0f);

            float currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, noiseFactor) * speed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, curveFactor);

            elapsedTime += Time.deltaTime * currentSpeed;

            yield return null;
        }
    }

    private Vector3 GetRandomPointInCollider(Collider collider)
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(collider.bounds.min.y, collider.bounds.max.y),
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
        );

        return randomPoint;
    }

    private void OnDrawGizmos()
    {
        // Draw the patrol area in the Unity editor
        if (patrolArea != null)
        {
            Gizmos.DrawWireCube(patrolArea.bounds.center, patrolArea.bounds.size);
        }
    }
}
