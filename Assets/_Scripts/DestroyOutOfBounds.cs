using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float xRange = 27f;
    private float yRange = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > xRange) 
        {
            Destroy(gameObject);
        }
        else if(transform.position.x < -xRange)
        {
            Destroy(gameObject);
        }
        if(transform.position.y < yRange)
        {
            Destroy(gameObject);
        }

    }
}
