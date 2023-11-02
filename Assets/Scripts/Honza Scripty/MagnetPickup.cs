using UnityEngine;

public class MagnetPickup : MonoBehaviour
{
    public float magnetRange = 5f;
    public float magnetSpeed = 5f;
    public float maxSpeed = 20f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= magnetRange)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float currentSpeed = Mathf.Lerp(magnetSpeed, maxSpeed, 1 - (distanceToPlayer / magnetRange));

            transform.position += directionToPlayer * currentSpeed * Time.deltaTime;
        }
    }
}
