using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow_1Movement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed = Random.Range(12f, 16f);
        transform.Translate(Vector3.left * speed * Time.deltaTime);

    }
}
