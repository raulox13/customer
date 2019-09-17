using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GIF : MonoBehaviour
{
    [SerializeField] private Texture t1, t2, t3;
    [SerializeField] private float timer = 6f;

    private Texture[] textures = new Texture[3];
    private int order = 0;
    private float initializer;

    // Start is called before the first frame update
    void Start()
    {
        initializer = timer;
        textures[0] = t1;
        textures[1] = t2;
        textures[2] = t3;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0f)
        {
            timer -= 1f * Time.deltaTime;
        }

        else
        {
            timer = initializer;
            order++;
            if (order > 2) order = 0;
            transform.GetComponent<RawImage>().texture = textures[order];
        }
    }
}
