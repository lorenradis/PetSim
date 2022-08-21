using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Affinity  {

    public string affinityName;
    private List<Affinity> strongAgainst = new List<Affinity>();
    private List<Affinity> weakAgainst = new List<Affinity>();

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