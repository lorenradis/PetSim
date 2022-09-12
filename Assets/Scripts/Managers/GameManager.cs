using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

using PetSim;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject playerPrefab;

    private Transform player;
    public Transform Player
    {
        get
        {
            if (player != null)
                return player;
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
                return player;
            }
            Debug.Log("Someone called the Player");
            GameObject playerObject = Instantiate(playerPrefab);
            player = playerObject.transform;
            return player;

        }
        set { }
    }

    //Game State Region
    public enum GameState { STARTUP, NORMAL, MENU, CUTSCENE, PAUSE, LOADING, BATTLE }
    public GameState gameState;
    private GameState previousState;
    private void ChangeState(GameState newState)
    {
        if (newState != gameState)
        {
            Debug.Log("Changing from " + gameState + " to " + newState);
            previousState = gameState;
            gameState = newState;
        }
    }

    public GameObject partnerPetPrefab;
    private List<GameObject> partnerPetObjects = new List<GameObject>();
    private List<PetInfo> partnerPets = new List<PetInfo>();
    private PetInfo encounteredPet;
    public PetInfo EncounteredPet { get { return encounteredPet; } set { encounteredPet = value; } }

    //Scene Change Region
    [SerializeField] private SceneInfo[] allScenes;
    [SerializeField] private Animator sceneTransitionAnimator;
    private SceneInfo currentScene;
    public SceneInfo CurrentScene { get { return currentScene; } set { } }
    private SceneInfo previousScene;
    private Vector2 playerStartPosition;
    public Vector2 PlayerStartPosition { get { return playerStartPosition; } set { playerStartPosition = value; } }
    private Vector2 playerFacing;
    private int lastEntrance = 0;
    public int LastEntrance { get { return lastEntrance; } }

    //scene persistence region?
    public List<GameObject> activeSpawnObjects = new List<GameObject>();
    public List<ActiveSpawnInfo> activeSpawns = new List<ActiveSpawnInfo>();
    public GameObject wildPetPrefab;
    public GameObject itemObjectPrefab;
    public List<Transform> baitObjects = new List<Transform>();

    //Managers Region

    public GameClock gameClock;
    public PetManager petManager;
    public AffinityManager affinityManager;
    public RegionManager regionManager;
    public UIManager uiManager;
    public TaskManager taskManager;
    public ItemManager itemManager;
    public FarmManager farmManager;
    public StoryProgression storyProgression;
    public PlayerInfo playerInfo;
    public GlobalLightManager globalLightManager;
    public AudioManager audioManager;
    public SaveManager saveManager;

    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("There was an extra GameManager");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        SetupClock();
        affinityManager = new AffinityManager();
        affinityManager.SetupAffinities();
        audioManager = GetComponent<AudioManager>();
        itemManager.SetupItems();
        petManager.SetupPets();
        regionManager = new RegionManager();
        regionManager.SetupRegions();
        taskManager = new TaskManager();
        taskManager.SetupTasks();
        //uiManager = new UIManager();
        uiManager.Setup();
        farmManager.SetupFarm(25, 15);
        playerInfo = new PlayerInfo(3, 960);
        globalLightManager.Setup();
        saveManager = new SaveManager();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void SetupClock()
    {
        gameClock = new GameClock();
        gameClock.SetTime(480);
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.STARTUP:
                break;
            case GameState.NORMAL:
                gameClock.Tick();
                break;
            case GameState.MENU:
                break;
            case GameState.PAUSE:
                break;
            default:
                break;
        }
        ManageInput();
    }

    private void ManageInput()
    {
        switch (gameState)
        {
            case GameState.STARTUP:
                break;
            case GameState.NORMAL:
                if (Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(KeyCode.Return))
                {
                    PauseGame();
                }
                break;
            case GameState.MENU:
                if(Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Escape))
                {
                    CloseCurrentMenu();
                }
                break;
            case GameState.PAUSE:
                if (Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(KeyCode.Return))
                {
                    //UnpauseGame();
                }
                break;
            default:
                break;
        }
    }

    private void PauseGame()
    {
        ShowSystemMenu();
    }

    private void UnpauseGame()
    {
        HideSystemMenu();
    }

    public void EnterMenuState()
    {
        ChangeState(GameState.MENU);
    }

    public void ExitMenuState()
    {
        ChangeState(previousState);
    }

    public void EnterCutsceneState()
    {
        ChangeState(GameState.CUTSCENE);
    }

    public void ExitCutsceneState()
    {
        ChangeState(GameState.NORMAL);
    }

    public void ShowPetsInfo()
    {
        uiManager.ShowPetsInfo();
        EnterMenuState();
    }

    public void HidePetsInfo()
    {
        uiManager.HidePetsInfo();
    }

    public void ShowPetInfo(PetInfo petInfo)
    {
        uiManager.ShowPetInfo(petInfo);
        EnterMenuState();
    }

    public void HidePetInfo()
    {
        uiManager.HidePetInfo();
        ExitMenuState();
    }

    public void ShowMapForPlayerMovement()
    {
        uiManager.ShowMapPanel();
        EnterMenuState();
    }

    public void ShowMapForAssignment()
    {
        uiManager.ShowMapPanel();
        uiManager.SetupMapForAssignment();
        EnterMenuState();
    }

    public void HideMap()
    {
        uiManager.HideMapPanel();
        ExitMenuState();
    }

    public void ShowFarmInfo()
    {
        ChangeState(GameState.MENU);
        uiManager.ShowFarmInfo();
    }

    public void HideFarmInfo()
    {
        ChangeState(GameState.NORMAL);
        uiManager.HideFarmInfo();
    }

    public void ShowIngameMenu()
    {
        uiManager.ShowIngameMenu();
        ChangeState(GameState.MENU);
    }

    public void HideIngameMenu()
    {
        uiManager.HideIngameMenu();
        ChangeState(GameState.NORMAL);
    }

    public void ShowItems()
    {
        HideIngameMenu();
        uiManager.ShowItems();
        ChangeState(GameState.MENU);
    }

    public void HideInventory()
    {
        uiManager.HideItems();
        ChangeState(GameState.NORMAL);
    }

    public void ShowResources()
    {
        uiManager.ShowResources();
        ChangeState(GameState.MENU); 
    }

    public void HideResources()
    {
        uiManager.HideResources();
        ChangeState(GameState.NORMAL);
    }

    public void ShowFoods()
    {
        uiManager.ShowFoods();
        ChangeState(GameState.MENU);
    }

    public void HideFoods()
    {
        uiManager.HideFoods();
        ChangeState(GameState.NORMAL);
    }
    public void ShowRegionInfo()
    {
        uiManager.ShowRegionInfo();
        ChangeState(GameState.MENU);
    }

    public void HideRegionInfo()
    {
        uiManager.HideRegionInfo();
        ChangeState(GameState.NORMAL);
    }

    public void ShowSystemMenu()
    {
        uiManager.ShowSystemMenu();
        ChangeState(GameState.MENU);
    }

    public void HideSystemMenu()
    {
        uiManager.HideSystemMenu();
        ChangeState(GameState.NORMAL);
    }

    public void CloseCurrentMenu()
    {
        ChangeState(uiManager.CloseCurrentMenu());
    }

    public void ChooseStarter()
    {
        ChangeState(GameState.MENU);
        uiManager.ShowStarterSelectScreen();
    }

    public void PlaceItem(Item item, Vector2 location)
    {
        GameObject newItemObject = Instantiate(itemObjectPrefab, location, Quaternion.identity) as GameObject;
        newItemObject.GetComponent<ItemObject>().SetItem(item);
    }

    public void StartBattleWithPet(GameObject petObject)
    {
        encounteredPet = petObject.GetComponent<OverworldPet>().petInfo;
        activeSpawnObjects.Remove(petObject);
        StartCoroutine(FadeToBattle());
        for (int i = 0; i < activeSpawnObjects.Count; i++)
        {
            ActiveSpawnInfo newSpawnInfo = new ActiveSpawnInfo();
            newSpawnInfo.petInfo = activeSpawnObjects[i].GetComponent<OverworldPet>().petInfo;
            newSpawnInfo.myPosition = activeSpawnObjects[i].transform.position;
            activeSpawns.Add(newSpawnInfo);
        }

        activeSpawnObjects.Clear();
    }

    public void ReturnFromBattle()
    {
        StartCoroutine(FadeToNewScene(previousScene.sceneName));
    }

    public void ReturnFromFainting()
    {
        StartCoroutine(FadeToNewScene(previousScene.sceneName));
    }

    public void GoToSleep()
    {
        StartCoroutine(RenderPlayerSleep());
    }

    private IEnumerator RenderPlayerSleep()
    {
        sceneTransitionAnimator.SetTrigger("fadeOut");
        //play sleepy song
        yield return new WaitForSeconds(.5f);

        int duration = 8 * 60;

        gameClock.AdvanceTime(duration);

        playerInfo.IncreaseEnergy(1000);
        playerInfo.IncreaseHealth(1000);

        sceneTransitionAnimator.SetTrigger("fadeIn");
    }

    public void LoadNewScene(SceneInfo sceneInfo, int entrance)
    {
        previousScene = currentScene;
        currentScene = sceneInfo;
        lastEntrance = entrance;
        playerStartPosition = sceneInfo.entrances[entrance];
        playerFacing = player.GetComponent<PlayerControls>().FacingVector;
        StartCoroutine(FadeToNewScene(sceneInfo.sceneName));
    }

    public void LoadNewScene(SceneInfo sceneInfo, Vector2 newPlayerPosition)
    {
        previousScene = currentScene;
        currentScene = sceneInfo;
        playerStartPosition = newPlayerPosition;
        playerFacing = player.GetComponent<PlayerControls>().FacingVector;
        StartCoroutine(FadeToNewScene(sceneInfo.sceneName));
    }

    private IEnumerator FadeToNewScene(string sceneName)
    {
        if(gameState != GameState.BATTLE)
        {
            activeSpawns.Clear();
        }
        ChangeState(GameState.LOADING);
        sceneTransitionAnimator.SetTrigger("fadeOut");
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeToBattle()
    {
        sceneTransitionAnimator.SetTrigger("fadeOut");
        //play encounter sound
        playerStartPosition = player.position;
        playerFacing = player.GetComponent<PlayerControls>().FacingVector;
        previousScene = currentScene;
        ChangeState(GameState.BATTLE);
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene("BattleScene");
        sceneTransitionAnimator.SetTrigger("fadeIn");
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        baitObjects.Clear();
        if(gameState != GameState.BATTLE)
        {
            Camera.main.GetComponent<CameraMovement>().SetMinBounds(currentScene.minBounds);
            Camera.main.GetComponent<CameraMovement>().SetMaxBounds(currentScene.maxBounds);
            StartCoroutine(FadeIn());
            Player.position = playerStartPosition;
            player.GetComponent<PlayerControls>().FacingVector = playerFacing;
            if(petManager.PartnerPet1 != null || petManager.PartnerPet2 != null)
            {
                SpawnPartnerPets();
            }

            for (int i = 0; i < activeSpawns.Count; i++)
            {
                GameObject newWildPet = Instantiate(wildPetPrefab, activeSpawns[i].myPosition, Quaternion.identity) as GameObject;
                activeSpawnObjects.Add(newWildPet);
                newWildPet.GetComponent<OverworldPet>().SetPetInfo(activeSpawns[i].petInfo);
            }

            activeSpawns.Clear();

            if(GameObject.FindGameObjectWithTag("Farm") != null)
            {
                uiManager.ShowFarmControls();
            }
            else
            {
                uiManager.HideFarmControls();
            }
        }
    }

    public void SpawnPartnerPets()
    {
        if(partnerPetObjects.Count > 0)
        {
            foreach(GameObject pet in partnerPetObjects)
            {
                Destroy(pet);
            }
            partnerPetObjects.Clear();
        }

        GameObject newPartner;

        if(petManager.PartnerPet1 != null)
        {
            Debug.Log(petManager.PartnerPet1.petName);
            newPartner = Instantiate(partnerPetPrefab, player.position, Quaternion.identity) as GameObject;
            partnerPetObjects.Add(newPartner);
            newPartner.GetComponent<CompanionPet>().SetPetInfo(petManager.PartnerPet1);
        }
        if(petManager.PartnerPet2 != null)
        {
            Debug.Log(petManager.PartnerPet2.petName);
            newPartner = Instantiate(partnerPetPrefab, player.position, Quaternion.identity) as GameObject;
            partnerPetObjects.Add(newPartner);
            newPartner.GetComponent<CompanionPet>().SetPetInfo(petManager.PartnerPet2);
        }
    }

    private IEnumerator FadeIn()
    {
        sceneTransitionAnimator.SetTrigger("fadeIn");
        yield return new WaitForSeconds(.5f);
        ChangeState(GameState.NORMAL);
    }

    public void SaveGame()
    {
        Dialog confirmSaveDialog = new Dialog("Save your current game?", "", null, false, "Yes", "No", "", "");
        DialogManager.instance.ShowDialog(confirmSaveDialog, () => {
            saveManager.SaveGameData();
        }, () => {
            HideSystemMenu();
        });
    }

    public void LoadGame()
    {
        Dialog confirmLoadDialog = new Dialog("Reload from your most recent save?", "", null, false, "Yes", "No", "", "");
        DialogManager.instance.ShowDialog(confirmLoadDialog, () => {
            saveManager.LoadGameData();
        }, () => {
            HideSystemMenu();
        });

    }
}

namespace PetSim
{
    [System.Serializable]
    public struct ProjectCost
    {
        public Item item;
            public int quantity;
    }

    [System.Serializable]
    public struct ActiveSpawnInfo
    {
        public Vector2 myPosition;
        public PetInfo petInfo;
    }
}