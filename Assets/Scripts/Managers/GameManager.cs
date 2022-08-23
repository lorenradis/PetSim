using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

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
            GameObject playerObject = Instantiate(playerPrefab);
            player = playerObject.transform;
            return player;

        }
        set { }
    }

    //Game State Region
    public enum GameState { STARTUP, NORMAL, MENU, PAUSE, LOADING }
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

    //Scene Change Region
    [SerializeField] private Animator sceneTransitionAnimator;
    private SceneInfo currentScene;
    private SceneInfo previousScene;
    private Vector2 playerStartPosition;
    private Vector2 playerFacing;

    //Managers Region

    public GameClock gameClock;
    public PetManager petManager;
    public AffinityManager affinityManager;
    public RegionManager regionManager;
    public UIManager uiManager;
    public TaskManager taskManager;
    public ItemManager itemManager;
    public FarmManager farmManager;

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
        petManager.SetupPets();
        itemManager.SetupItems();
        regionManager = new RegionManager();
        regionManager.SetupRegions();
        taskManager = new TaskManager();
        taskManager.SetupTasks();
        //uiManager = new UIManager();
        uiManager.Setup();
        farmManager.SetupFarm(44, 44);
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
                    UnpauseGame();
                }
                break;
            default:
                break;
        }
    }

    private void PauseGame()
    {
        ChangeState(GameState.PAUSE);
    }

    private void UnpauseGame()
    {
        ChangeState(GameState.NORMAL);
    }

    public void EnterMenuState()
    {
        ChangeState(GameState.MENU);
    }

    public void ExitMenuState()
    {
        ChangeState(previousState);
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

    public void ShowInventory()
    {
        HideIngameMenu();
        uiManager.ShowInventory();
        ChangeState(GameState.MENU);
    }

    public void HideInventory()
    {
        uiManager.HideInventory();
    }

    public void CloseCurrentMenu()
    {
        ChangeState(uiManager.CloseCurrentMenu());
    }

    public void LoadNewScene(SceneInfo sceneInfo, int entrance)
    {
        previousScene = currentScene;
        currentScene = sceneInfo;
        playerStartPosition = sceneInfo.entrances[entrance];
        playerFacing = player.GetComponent<PlayerControls>().FacingVector;
        ChangeState(GameState.LOADING);
        StartCoroutine(FadeToNewScene(sceneInfo.sceneName));
    }

    private IEnumerator FadeToNewScene(string sceneName)
    {
        sceneTransitionAnimator.SetTrigger("fadeOut");
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
        Player.position = playerStartPosition;
        player.GetComponent<PlayerControls>().FacingVector = playerFacing;
    }

    private IEnumerator FadeIn()
    {
        sceneTransitionAnimator.SetTrigger("fadeIn");
        yield return new WaitForSeconds(.5f);
        ChangeState(GameState.NORMAL);
    }
}
