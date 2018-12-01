﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSSlider : MonoBehaviour
{

    public Slider slider;
    public Text text;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeSlider()
    {
        text.text = "FPS : " + (int)slider.value;
        Application.targetFrameRate = (int)slider.value;
    }
}
