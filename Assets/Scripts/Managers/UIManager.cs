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


    public UIManager()
    {

    }

    public void Setup()
    {
        GameClock.onMinuteChangedCallback += DisplayTime;
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
}
