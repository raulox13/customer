using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbitate : MonoBehaviour
{
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.value + 1f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
