﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightIntensity : MonoBehaviour
{
    [SerializeField] private Slider slider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(slider.value);
        RenderSettings.ambientLight = new Color(slider.value * 100, slider.value * 100, slider.value * 100);
        GetComponent<Light>().intensity = slider.value + 0.5f;
    }
}
