using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpecificObject : MonoBehaviour
{
    [SerializeField] private int regionIndex;
    [SerializeField] private int objectIndex;
    private bool isActive;

    private void Start()
    {
        //pass my index int into my region to find out if I am active or not.
        //region needs an array of bools to return;
        isActive = GameManager.instance.regionManager.regions[regionIndex].IsObjectActive(objectIndex);
        gameObject.SetActive(isActive);
    }
}
