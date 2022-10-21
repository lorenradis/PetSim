using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class SaveManager
{

    //create a SaveManager saveManager in the GameManager instance.

    public SaveData activeSave;

    public bool hasLoaded = false;

    public void SaveGameData()
    {

        activeSave = new SaveData();

        string dataPath = Application.persistentDataPath;

        var serializer = new XmlSerializer(typeof(SaveData));
        var stream = new FileStream(dataPath + "/" + activeSave.saveName + ".sav", FileMode.Create);

        activeSave.playerPosition = GameManager.instance.Player.position;

        activeSave.playerEnergy = GameManager.instance.playerInfo.energy;
        activeSave.playerHealth = GameManager.instance.playerInfo.health;

        activeSave.currentScene = GameManager.instance.CurrentScene;
        activeSave.lastEntrance = GameManager.instance.LastEntrance;

        activeSave.storyProgression = GameManager.instance.storyProgression;
        for (int x = 0; x < GameManager.instance.farmManager.Width; x++)
        {
            for (int y = 0; y < GameManager.instance.farmManager.Height; y++)
            {
                activeSave.farmTiles.Add(GameManager.instance.farmManager.gridTiles[x, y]);
            }
        }

        activeSave.time = GameManager.instance.gameClock.Ticks;

        activeSave.grassFoodCount = ItemManager.grassFood.quantity;
        activeSave.waterFoodCount = ItemManager.waterFood.quantity;
        activeSave.fireFoodCount = ItemManager.fireFood.quantity;

        /*
        for (int i = 0; i < GameManager.instance.petManager.currentPets.Count; i++)
        {
            activeSave.ownedPets.Add(GameManager.instance.petManager.currentPets[i]);
        }
        */

        serializer.Serialize(stream, activeSave);
        stream.Close();
        
    }

    public void LoadGameData()
    {
        
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName + ".sav"))
        {
            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(dataPath + "/" + activeSave.saveName + ".sav", FileMode.Open);
            activeSave = serializer.Deserialize(stream) as SaveData;

            GameManager.instance.PlayerStartPosition = activeSave.playerPosition;

            GameManager.instance.playerInfo.energy = activeSave.playerEnergy;
            GameManager.instance.playerInfo.health = activeSave.playerHealth;

            GameManager.instance.storyProgression = activeSave.storyProgression;

            int tileIndex = 0;
            for (int x = 0; x < GameManager.instance.farmManager.Width; x++)
            {
                for (int y = 0; y < GameManager.instance.farmManager.Height; y++)
                {
                    GameManager.instance.farmManager.gridTiles[x, y] = activeSave.farmTiles[tileIndex];
                    tileIndex++;
                }
            }


            ItemManager.grassFood.quantity = activeSave.grassFoodCount;
            ItemManager.waterFood.quantity = activeSave.waterFoodCount;
            ItemManager.fireFood.quantity = activeSave.fireFoodCount;

            GameManager.instance.gameClock.SetTime(activeSave.time);

            /*
            for (int i = 0; i < activeSave.ownedPets.Count; i++)
            {
                GameManager.instance.petManager.currentPets.Add(activeSave.ownedPets[i]);
            }
            */

            GameManager.instance.LoadNewScene(activeSave.currentScene, activeSave.playerPosition);

            stream.Close();
            hasLoaded = true;
        }
        else
        {

        }
        
    }

    public void DeleteSaveData()
    {
        
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName + ".sav"))
        {
            File.Delete(dataPath + "/" + activeSave.saveName + ".sav");
        }
        else
        {

        }
        
    }

}

[System.Serializable]
public class SaveData
{

    public SaveData() { }

    public string saveName;

    //player info

    public Vector2 playerPosition;

    public int playerHealth;
    public int playerEnergy;
    public int playerMoney;

    //world info

    public SceneInfo currentScene;
    public int time;
    public int lastEntrance;

    public StoryProgression storyProgression;

    public List<FarmManager.TileState> farmTiles = new List<FarmManager.TileState>();

    //public List<PetInfo> ownedPets = new List<PetInfo>();

    public int grassFoodCount;
    public int waterFoodCount;
    public int fireFoodCount;
}
