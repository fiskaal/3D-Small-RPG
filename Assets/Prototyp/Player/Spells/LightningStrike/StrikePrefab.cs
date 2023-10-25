using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikePrefab : MonoBehaviour
{
    private Animation _animation;

    [SerializeField]private float animationTime = 1f;
    private float timepassed;
    // Start is called before the first frame update
    void Start()
    {
        _animation = gameObject.GetComponent<Animation>();
        _animation.Play();
        timepassed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (animationTime <= timepassed)
        {
            Destroy(this);
        }

        timepassed += Time.deltaTime;
    }
}
