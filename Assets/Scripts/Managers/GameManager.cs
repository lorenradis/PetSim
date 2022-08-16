using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public GameClock gameClock;
    public PetManager petManager;
    public AffinityManager affinityManager;
    public RegionManager regionManager;

    public enum GameState{STARTUP, NORMAL, MENU, PAUSE}
    public GameState gameState;
    private GameState previousState;
    private void ChangeState(GameState newState)
    {
        if(newState != gameState)
        {
            previousState = gameState;
            gameState = newState;
        }
    }

    public static GameManager instance = null;

    private void Awake()
    {
        if(instance = null)
        {
            instance = this;
        }else{
            Debug.Log("There was an extra GameManager");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
            SetupClock();
            affinityManager = new AffinityManager();
            affinityManager.SetupAffinities();
            petManager = new PetManager();
            petManager.SetupPets();
            regionManager = new RegionManager();
            regionManager.SetupRegions();
    }

    private void SetupClock()
    {
        gameClock = new GameClock();
        gameClock.SetTime(480);
    }

    private void Update()
    {
        switch(gameState)
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
        switch(gameState)
        {
            case GameState.STARTUP:
                break;
            case GameState.NORMAL:
                if(Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(Keycode.Enter))
                {
                    PauseGame();
                }
                break;
            case GameState.MENU:
                break;
            case GameState.PAUSE:
                if(Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(Keycode.Enter))
                {
                    UnpauseGame();
                }
                break;
            default:
                break;
        }
    }
    
    public void EnterMenuState()
    {
        ChangeState(GameState.MENU);
    }

    public void ExitMenuState()
    {
        ChangeState(previousState);
    }

}
