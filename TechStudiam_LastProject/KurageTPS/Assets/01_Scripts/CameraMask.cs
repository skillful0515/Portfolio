﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMask : MonoBehaviour
{
    public Material mat;

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }
}
