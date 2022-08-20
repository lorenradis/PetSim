using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleUIAnimation : MonoBehaviour
{
    float frequency = 12f/60f;
    float elapsedTime = 0f;

    [SerializeField] private Sprite[] frames;

    private int index = 0;

    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > frequency)
        {
            index++;
            if (index >= frames.Length)
                index = 0;
            elapsedTime -= frequency;
            image.sprite = frames[index];
        }
    }
}
