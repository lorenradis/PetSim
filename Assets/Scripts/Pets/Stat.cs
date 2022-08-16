using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Stat{

    private string statName;
    public string StatName{get{return statName}set{}}
    [SerializeField] private int baseValue; //creature specific starting value
    public int BaseValue{get{return baseValue;}set{}}
    private int randomValue; //randomly generated on instantiate
    private int modifier = 0;
    private int permModifier = 0;
    private float multiplier = 1f;
    [SerializeField] private int value;
    public int Value{get{
        value = Mathf.FloorToInt(baseValue + randomValue + modifier + permModifier * multiplier);
        return value;
    } set{}}

    public Stat()
    {

    }

    public Stat(string newName, int newBase)
    {
        statName = newName;
        baseValue = newBase;
        modifier = 0;
        multiplier = 1f;
        randomValue = Random.Range(1, 6) + Random.Range(1, 6) + Random.Range(1, 6);
    }

}