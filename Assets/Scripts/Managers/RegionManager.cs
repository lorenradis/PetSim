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
        desertRegion = new Region("Desert", AffinityManager.fireAffinity, ItemManager.sand, ItemManager.succulent);
        forestRegion = new Region("Forest", AffinityManager.grassAffinity, ItemManager.wood, ItemManager.berries);
        lakeRegion = new Region("Lake", AffinityManager.waterAffinity, ItemManager.water, ItemManager.mushrooms);

        forestRegion.AddLevelUnlock(new LevelUnlock(3, () => { forestRegion.levelSpecificObjects[0] = false; }, false), true); //exit from forest to lake
        forestRegion.AddLevelUnlock(new LevelUnlock(4, () => { forestRegion.levelSpecificObjects[1] = true; }, false), false); //doorway to house

        forestRegion.AddSpawn(PetManager.Bulbos, false, 0);
        forestRegion.AddSpawn(PetManager.Bulbos, true, 1);
        forestRegion.AddSpawn(PetManager.Squirt, false, 1);
        forestRegion.AddSpawn(PetManager.Squirt, true, 0);
        forestRegion.AddSpawn(PetManager.Charby, false, 2);
        forestRegion.AddSpawn(PetManager.Bulbos, true, 2);
        forestRegion.AddSpawn(PetManager.Squirt, true, 2);
        forestRegion.AddSpawn(PetManager.Stunky, false, 2);
        forestRegion.AddSpawn(PetManager.Stunky, true, 2);

        regions.Add(forestRegion);
        regions.Add(lakeRegion);
        regions.Add(desertRegion);

        forestRegion.UnlockRegion();
        desertRegion.UnlockRegion();
        lakeRegion.UnlockRegion();
    }

    public void SelectRegion(Region region)
    {
        selectedRegion = region;
    }
}