using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Affinity  {

    public string affinityName;
    public List<Affinity> strongAgainst = new List<Affinity>();
    public List<Affinity> weakAgainst = new List<Affinity>();

    public Affinity()
    {

    }

    public Affinity(string newName, List<Affinity> strengths, List<Affinity> weaknesses)
    {
        affinityName = newName;
        strongAgainst = strengths;
        weakAgainst = weaknesses;
    }

}