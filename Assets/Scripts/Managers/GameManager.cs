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

    public enum GameState { STARTUP, NORMAL, MENU, PAUSE }
    public GameState gameState;
    private GameState previousState;
    private void ChangeState(GameState newState)
    {
        if (newState != gameState)
        {
            previousState = gameState;
            gameState = newState;
        }
    }

    public GameClock gameClock;
    public PetManager petManager;
    public AffinityManager affinityManager;
    public RegionManager regionManager;
    public UIManager uiManager;

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
        petManager = new PetManager();
        petManager.SetupPets();
        regionManager = new RegionManager();
        regionManager.SetupRegions();
        //uiManager = new UIManager();
        uiManager.Setup();
    }

    private void Start()
    {

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
}
