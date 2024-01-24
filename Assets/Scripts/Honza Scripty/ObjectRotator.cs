using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
