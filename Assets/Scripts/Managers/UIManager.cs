using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using TMPro;

[System.Serializable]
public class UIManager 
{
    [SerializeField] private TextMeshProUGUI timeText;

    //pet info region

    [SerializeField] private GameObject petInfoPanel;
    [SerializeField] private Image petPortraitImage;
    [SerializeField] private TextMeshProUGUI petNameText;
    [SerializeField] private TextMeshProUGUI petStrengthText;
    [SerializeField] private TextMeshProUGUI petSmartsText;
    [SerializeField] private TextMeshProUGUI petSpeedText;
    [SerializeField] private TextMeshProUGUI petLuckText;
    [SerializeField] private TextMeshProUGUI petHungerText;
    [SerializeField] private TextMeshProUGUI petBoredomText;
    [SerializeField] private TextMeshProUGUI petCleanText;
    [SerializeField] private TextMeshProUGUI petHappyText;
    [SerializeField] private TextMeshProUGUI petAffinityText;
    [SerializeField] private TextMeshProUGUI petAgeText;
    [SerializeField] private TextMeshProUGUI petTaskText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button closePetInfoPanelButton;

    //map region
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private Button region1Button;
    [SerializeField] private Button region2Button;
    [SerializeField] private Button region3Button;
    [SerializeField] private Button[] mapButtons = new Button[3];
    [SerializeField] private Button mapCloseButton;

    public void Setup()
    {
        GameClock.onMinuteChangedCallback += DisplayTime;
        mapButtons[0] = region1Button;
        mapButtons[1] = region2Button;
        mapButtons[2] = region3Button;

    }

    private void DisplayTime()
    {
        timeText.text = GameManager.instance.gameClock.StringTime;
    }

    public void ShowPetInfo(PetInfo petInfo)
    {
        petInfoPanel.SetActive(true);
        petNameText.text = petInfo.petName;
        petStrengthText.text = "Strength: " + petInfo.Strength.Value;
        petSpeedText.text = "Smarts: " + petInfo.Smarts.Value;
        petSmartsText.text = "Speed: " + petInfo.Speed.Value;
        petLuckText.text = "Luck: " + petInfo.Luck.Value;

        petHungerText.text = "Food: " + petInfo.food;
        petBoredomText.text = "Play: " + petInfo.play;
        petCleanText.text = "Clean: " + petInfo.clean;
        petHappyText.text = "Happy: ";


        petAffinityText.text = "Affinity: " + petInfo.affinity.affinityName;
        petAgeText.text = "Age: ";
        petTaskText.text = "Current Task: ";

        descriptionText.text = petInfo.description;

        EventSystem.current.SetSelectedGameObject(closePetInfoPanelButton.gameObject);
    }

    public void HidePetInfo()
    {
        petInfoPanel.SetActive(false);
    }

    public void ShowMapPanel()
    {
        mapPanel.SetActive(true);
        for (int i = 0; i < GameManager.instance.regionManager.regions.Count; i++)
        {
            if (GameManager.instance.regionManager.regions[i].isUnlocked)
            {
                int index = i;
                mapButtons[i].enabled = true;
                mapButtons[i].onClick.RemoveAllListeners();
                mapButtons[i].onClick.AddListener(() => { Debug.Log("Selecting region " + index); GameManager.instance.regionManager.SelectRegion(GameManager.instance.regionManager.regions[index]); });
                mapButtons[i].onClick.AddListener(() => { HideMapPanel();  ShowAvailableTasks(GameManager.instance.regionManager.SelectedRegion); });
            }
            else
            {
                mapButtons[i].enabled = false;
            }

        }
        mapCloseButton.onClick.AddListener(() => GameManager.instance.HideMap());
        EventSystem.current.SetSelectedGameObject(mapButtons[0].gameObject);
    }

    public void SetupMapForAssignment()
    {
        for (int i = 0; i < mapButtons.Length; i++)
        {
            mapButtons[i].onClick.AddListener(() => { });
        }
    }

    private void AddTask(Task task, Region region, PetInfo petInfo)
    {

    }

    public void HideMapPanel()
    {
        mapPanel.SetActive(false);

    }

    public void ShowAvailableTasks(Region region)
    {
        Dialog dialog = new Dialog("What would you like so and so to do there?", "", null, false, "Gather Resources", "Cancel", "", "");
        DialogManager.instance.ShowDialog(dialog, () => {
            DialogManager.instance.ShowSimpleDialog("So and so went off on assignment, so long so and so!");
            GameManager.instance.petManager.SelectedPet.AssignTask(GameManager.instance.taskManager.GatherResources);

        }, () => { }
            
        );
    }

    public void HideAvailableTasks()
    {

    }
}
