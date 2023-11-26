using System.Collections.Generic;
using UnityEngine;

public class BossDamageDealer : MonoBehaviour
{
    bool canDealDamage;
    List<GameObject> hasDealtDamage;

    [SerializeField] public float attackDamage;
    [SerializeField] int rayCount = 12; // Number of raycasts for the circle
    [SerializeField] float circleRadius = 3f; // Radius of the circular area
    [SerializeField] ParticleSystem warningEffect; // Particle system for warning effect

    public EnemyBoss enemyBoss;
    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();

        if (warningEffect != null)
            warningEffect.Stop(); // Ensure the warning effect is initially stopped
    }

    //must be called before initiating attack for the correct damage to be applied
    public void ChangeDamage(float damage, float radius)
    {
        attackDamage = damage;
        if (radius != 0)
        {
            circleRadius = radius;
        }
    }

    void Update()
    {
        if (canDealDamage)
        {
            CastCircularArea();
        }
    }

    void CastCircularArea()
    {
        float angleStep = 360f / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = Quaternion.AngleAxis(angle, transform.up) * -transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, circleRadius))
            {
                // Deal damage to enemies within the circular area
                if (hit.transform.TryGetComponent(out HealthSystem player) && !hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    player.TakeDamage(attackDamage, transform, hit.transform);
                    player.HitVFX(hit.point);
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
            }
        }
    }

    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealtDamage.Clear();

        PlayWarningEffect();
    }

    public void EndDealDamage()
    {
        canDealDamage = false;
        hasDealtDamage.Clear();

        if (warningEffect != null)
            warningEffect.Stop(); // Stop the warning effect when dealing damage ends
    }

    public void PlayWarningEffect()
    {
        if (warningEffect != null)
        {
            warningEffect.transform.position = transform.position;
            warningEffect.transform.rotation = Quaternion.identity;
            warningEffect = Instantiate(warningEffect);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        float angleStep = 360f / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = Quaternion.AngleAxis(angle, transform.up) * -transform.forward;
            Gizmos.DrawLine(transform.position, transform.position + direction * circleRadius);
        }
    }
}
