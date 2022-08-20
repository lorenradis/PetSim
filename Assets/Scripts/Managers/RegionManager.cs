using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RegionManager  {

    public Region desertRegion;
    public Region forestRegion;
    public Region lakeRegion;

    private Region selectedRegion;
    public Region SelectedRegion { get { return selectedRegion; } set { } }

    public List<Region> regions = new List<Region>();

    public RegionManager()
    {

    }

    public void SetupRegions()
    {
        desertRegion = new Region("Desert", AffinityManager.fireAffinity);
        forestRegion = new Region("Forest", AffinityManager.grassAffinity);
        lakeRegion = new Region("Lake", AffinityManager.waterAffinity);

        forestRegion.UnlockRegion();

        regions.Add(forestRegion);
        regions.Add(lakeRegion);
        regions.Add(desertRegion);
    }

    public void SelectRegion(Region region)
    {
        selectedRegion = region;
    }
}