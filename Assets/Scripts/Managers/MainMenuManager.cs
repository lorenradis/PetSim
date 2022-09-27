using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    private bool isActive = true;

    [SerializeField] private SceneInfo startScene;

    [SerializeField] private Button newGameButton;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);
    }

    public void StartNewGame()
    {
        if (!isActive)
            return;
        isActive = false;
        Debug.Log("Starting new game!");
        GameManager.instance.LoadNewScene(startScene, 0);
    }



    public void LoadSavedGame()
    {
        if (!isActive)
            return;
        isActive = false;

        GameManager.instance.LoadGame();
    }

    public void ShowOptions()
    {
        if (!isActive)
            return;
        isActive = false;

    }

    public void QuitGame()
    {
        if (!isActive)
            return;
        isActive = false;
        Application.Quit();
    }
}
