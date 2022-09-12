using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeSensitiveLight : MonoBehaviour
{
    public Color globalColor;

    private Light2D light;

    private void Start()
    {
        light = GetComponent<Light2D>();
    }

    private void Update()
    {
        globalColor = GameManager.instance.globalLightManager.currentColor;
        light.color = Color.white - globalColor;
    }
}
