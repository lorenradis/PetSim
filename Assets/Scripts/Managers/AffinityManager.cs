using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AffinityManager {

    public List<Affinity> affinities = new List<Affinity>();

    public static Affinity grassAffinity;
    public static Affinity waterAffinity;
    public static Affinity fireAffinity;
    public static Affinity earthAffinity;
    public static Affinity windAffinity;
    public static Affinity darkAffinity;
    public static Affinity lightAffinity;

    public AffinityManager()
    {

    }

    public void SetupAffinities()
    {
        List<Affinity> strengths = new List<Affinity>();
        List<Affinity> weaknesses = new List<Affinity>();

        strengths.Add(waterAffinity);
        strengths.Add(earthAffinity);
        weaknesses.Add(fireAffinity);
        weaknesses.Add(windAffinity);
        grassAffinity = new Affinity("Grass", strengths, weaknesses);

        strengths.Clear();
        weaknesses.Clear();

        strengths.Add(fireAffinity);
        strengths.Add(windAffinity);
        weaknesses.Add(grassAffinity);
        weaknesses.Add(earthAffinity);
        waterAffinity = new Affinity("Water", strengths, weaknesses);
        
        strengths.Clear();
        weaknesses.Clear();

        strengths.Add(grassAffinity);
        strengths.Add(earthAffinity);
        weaknesses.Add(waterAffinity);
        weaknesses.Add(windAffinity);
        fireAffinity = new Affinity("Fire", strengths, weaknesses);

        strengths.Clear();
        weaknesses.Clear();

        strengths.Add(waterAffinity);
        strengths.Add(windAffinity);
        weaknesses.Add(fireAffinity);
        weaknesses.Add(grassAffinity);
        earthAffinity = new Affinity("Earth", strengths, weaknesses);

        strengths.Clear();
        weaknesses.Clear();

        strengths.Add(fireAffinity);
        strengths.Add(grassAffinity);
        weaknesses.Add(earthAffinity);
        weaknesses.Add(waterAffinity);
        windAffinity = new Affinity("Wind", strengths, weaknesses);

        strengths.Clear();
        weaknesses.Clear();

        strengths.Add(darkAffinity);
        weaknesses.Add(lightAffinity);
        darkAffinity = new Affinity("Dark", strengths, weaknesses);

        strengths.Clear();
        weaknesses.Clear();

        strengths.Add(lightAffinity);
        weaknesses.Add(darkAffinity);
        lightAffinity = new Affinity("Light", strengths, weaknesses);

        affinities.Add(grassAffinity);
        affinities.Add(waterAffinity);
        affinities.Add(fireAffinity);
        affinities.Add(earthAffinity);
        affinities.Add(windAffinity);
        affinities.Add(darkAffinity);
        affinities.Add(lightAffinity);

    }

}