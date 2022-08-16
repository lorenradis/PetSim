using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AffinityManager {

    public static Affinity grassAffinity;
    public static Affinity waterAffinity;
    public static Affinity fireAffinity;

    public AffinityManager()
    {

    }

    public void SetupAffinities()
    {
        List<Affinity> strengths = new List<Affinity>();
        List<Affinity> weaknesses = new List<Affinity>();
        strengths.Add(waterAffinity);
        weaknesses.Add(fireAffinity);
        grassAffinity = new Affinity("Grass", strengths, weaknesses);

        strengths.Clear();
        weaknesses.Clear();

        strengths.Add(fireAffinity);
        weaknesses.Add(grassAffinity);
        waterAffinity = new Affinity("Water", strengths, weaknesses);
        
        strengths.Clear();
        weaknesses.Clear();

        strengths.Add(grassAffinity);
        weaknesses.Add(waterAffinity);
        fireAffinity = new Affinity("Fire", strengths, weaknesses);
    }

}