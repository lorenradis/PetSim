using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using TMPro;

[Serializable]
public class UIManager
{
    [SerializeField] private TextMeshProUGUI timeText;

    private int pageIndex = 0;

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

    //farm tools region
    [SerializeField] private GameObject farmControlsPanel;
    [SerializeField] private Image leftToolImage;
    [SerializeField] private Image centerToolImage;
    [SerializeField] private Image rightToolImage;
    [SerializeField] private TextMeshProUGUI toolText;

    //farm info region
    [SerializeField] private GameObject farmInfoPanel;
    [SerializeField] private TextMeshProUGUI farmNameText;
    [SerializeField] private TextMeshProUGUI farmInfoText;
    [SerializeField] private Button closeFarmInfoButton;

    //menu region
    [SerializeField] private GameObject inGameMenuPanel;
    [SerializeField] private Button[] menuButtons;

    //region info region
    [SerializeField] private GameObject regionInfoPanel;
    [SerializeField] private TextMeshProUGUI regionInfoText;
    [SerializeField] private Button closeRegionInfoButton;

    //inventory region
    [SerializeField] private GameObject itemsPanel;
    [SerializeField] private Button closeItemsButton;
    [SerializeField] private GameObject resourcesPanel;
    [SerializeField] private Button closeResourcesButton;
    [SerializeField] private GameObject foodsPanel;
    [SerializeField] private Button closeFoodsButton;

    [SerializeField] private ItemSlot[] itemSlots;
    [SerializeField] private ItemSlot[] resourceSlots;
    [SerializeField] private ItemSlot[] foodSlots;

    //startup screen region
    [SerializeField] private GameObject starterSelectPanel;
    [SerializeField] private Button starter1Button;
    [SerializeField] private Button starter2Button;
    [SerializeField] private Button starter3Button;

    //playerinfo region
    [SerializeField] private Slider energySlider;

    public void Setup()
    {
        GameClock.onMinuteChangedCallback += DisplayTime;
        PlayerInfo.onEnergyChangedCallback += DisplayPlayerEnergy;
        mapButtons[0] = region1Button;
        mapButtons[1] = region2Button;
        mapButtons[2] = region3Button;

    }

    private void DisplayTime()
    {
        timeText.text = GameManager.instance.gameClock.StringTime;
    }

    private void DisplayPlayerEnergy()
    {
        energySlider.maxValue = GameManager.instance.playerInfo.maxEnergy;
        energySlider.value = GameManager.instance.playerInfo.energy;
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
                mapButtons[i].onClick.AddListener(() => { HideMapPanel(); ShowAvailableTasks(GameManager.instance.regionManager.SelectedRegion); });
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

    public void HideMapPanel()
    {
        mapPanel.SetActive(false);

    }

    public void ShowAvailableTasks(Region region)
    {
        Dialog dialog = new Dialog("What would you like " + GameManager.instance.petManager.SelectedPet.petName + " to do there?", "", null, false, "Gather", "Forage", "Explore", "Cancel");
        DialogManager.instance.ShowDialog(dialog, () => {
            //if we choose gather
            DialogManager.instance.ShowSimpleDialog(GameManager.instance.petManager.SelectedPet.petName + " went out on assignment, good luck " + GameManager.instance.petManager.SelectedPet.petName + "!");
            GameManager.instance.petManager.SelectedPet.AssignTask(GameManager.instance.taskManager.GatherResources, GameManager.instance.regionManager.SelectedRegion);

        }, () => {
            //if we choose forage
            DialogManager.instance.ShowSimpleDialog(GameManager.instance.petManager.SelectedPet.petName + " went out on assignment, good luck " + GameManager.instance.petManager.SelectedPet.petName + "!");
            GameManager.instance.petManager.SelectedPet.AssignTask(GameManager.instance.taskManager.ForageFood, GameManager.instance.regionManager.SelectedRegion);
        }, () => {
            //if we choose explore
            DialogManager.instance.ShowSimpleDialog(GameManager.instance.petManager.SelectedPet.petName + " went out on assignment, good luck " + GameManager.instance.petManager.SelectedPet.petName + "!");
            GameManager.instance.petManager.SelectedPet.AssignTask(GameManager.instance.taskManager.Explore, GameManager.instance.regionManager.SelectedRegion);
        }, () => {
            //if we choose cancel
        });
    }

    public void ShowFarmInfo()
    {
        HideIngameMenu();
        farmInfoPanel.SetActive(true);
        farmInfoText.text = "Total Area : " + GameManager.instance.farmManager.farmTilesCount + "\n" +
            "Long Grass Tiles: " + GameManager.instance.farmManager.longGrassTilesCount + "\n" +
            "Desert Tiles: " + GameManager.instance.farmManager.desertTilesCount + "\n" +
            "Water Tiles: " + GameManager.instance.farmManager.waterTilesCount;
        EventSystem.current.SetSelectedGameObject(closeFarmInfoButton.gameObject);
    }

    public void HideFarmInfo()
    {
        farmInfoPanel.SetActive(false);
        ShowIngameMenu();
    }

    public void ShowIngameMenu()
    {
        inGameMenuPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(menuButtons[0].gameObject);
    }

    public void HideIngameMenu()
    {
        inGameMenuPanel.SetActive(false);
    }

    public void UpdateToolDisplay(List<FarmManager.TileState> tiles, int index)
    {
        centerToolImage.sprite = GameManager.instance.farmManager.GetSprite(tiles[index]);
        int left = index - 1;
        if (left < 0) left = tiles.Count - 1;
        leftToolImage.sprite = GameManager.instance.farmManager.GetSprite(tiles[left]);
        int right = index + 1;
        if (right >= tiles.Count) right = 0;
        rightToolImage.sprite = GameManager.instance.farmManager.GetSprite(tiles[right]);
        toolText.text = tiles[index].ToString();
    }

    public void HideAvailableTasks()
    {

    }

    public void ShowItems()
    {
        itemsPanel.SetActive(true);
        ItemManager inventory = GameManager.instance.itemManager;
        //there are buttons for showing items, foods and resources
        int slotIndex = 0;
        for (int i = 0; i < inventory.allItems.Count; i++)
        {
            if (inventory.allItems[i].quantity > 0)
            {
                itemSlots[slotIndex].SetItem(inventory.allItems[i]);
                itemSlots[slotIndex].gameObject.SetActive(true);
                slotIndex++;
            }
        }
        if(itemSlots[0].gameObject.activeSelf)
            EventSystem.current.SetSelectedGameObject(itemSlots[0].gameObject);
        else
        {
            EventSystem.current.SetSelectedGameObject(closeItemsButton.gameObject);
        }
    }

    public void HideItems()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].gameObject.SetActive(false);
        }
        itemsPanel.SetActive(false);
        ShowIngameMenu();
    }

    public void ShowResources()
    {
        resourcesPanel.SetActive(true);
        HideIngameMenu();
        int slotIndex = 0;
        ItemManager inventory = GameManager.instance.itemManager;
        for (int i = 0; i < inventory.allResources.Count; i++)
        {
            if (inventory.allResources[i].quantity > 0)
            {
                resourceSlots[slotIndex].SetItem(inventory.allResources[i]);
                resourceSlots[slotIndex].gameObject.SetActive(true);
                slotIndex++;
            }
        }

        if (resourceSlots[0].gameObject.activeSelf)
            EventSystem.current.SetSelectedGameObject(resourceSlots[0].gameObject);
        else
        {
            EventSystem.current.SetSelectedGameObject(closeResourcesButton.gameObject);
        }
    }

    public void HideResources()
    {
        for (int i = 0; i < resourceSlots.Length; i++)
        {
            resourceSlots[i].gameObject.SetActive(false);
        }
        resourcesPanel.SetActive(false);
        ShowIngameMenu();
    }

    public void ShowFoods()
    {
        int slotIndex = 0;
        ItemManager inventory = GameManager.instance.itemManager;
        foodsPanel.SetActive(true);
        HideIngameMenu();
        for (int i = 0; i < inventory.allFoods.Count; i++)
        {
            if (inventory.allFoods[i].quantity > 0)
            {
                foodSlots[slotIndex].SetItem(inventory.allFoods[i]);
                foodSlots[slotIndex].gameObject.SetActive(true);
                slotIndex++;
            }
        }
        if (foodSlots[0].gameObject.activeSelf)
            EventSystem.current.SetSelectedGameObject(foodSlots[0].gameObject);
        else
        {
            EventSystem.current.SetSelectedGameObject(closeFoodsButton.gameObject);
        }
    }

    public void HideFoods()
    {
        for (int i = 0; i < foodSlots.Length; i++)
        {
            foodSlots[i].gameObject.SetActive(false);
        }
        foodsPanel.SetActive(false);
        ShowIngameMenu();
    }

    public void ShowRegionInfo()
    {
        HideIngameMenu();
        regionInfoPanel.SetActive(true);
        regionInfoText.text = "";
        foreach (Region region in GameManager.instance.regionManager.regions)
        {
            regionInfoText.text += region.regionName + " - Affinity: " + region.affinity.affinityName + ", Level: " + region.Level + ", Experience: " + region.Experience + "/" + region.ToNextLevel + " \n";
        }
    }

    public void HideRegionInfo()
    {
        regionInfoPanel.SetActive(false);
    }

    public void ShowFarmControls()
    {
        farmControlsPanel.SetActive(true);
    }

    public void HideFarmControls()
    {
        farmControlsPanel.SetActive(false);
    }

    public void ChangePage(int amount)
    {
        /*
         * some ui gameobjects will be arrays rather than individual items - inventory for instance.
         * int pageIndex will reset to 0 every time a new menu is opened
         * showinventory() will reveal inventoryPanels[pageIndex]
         * then advancing the page index will first check if the currently active panel is one of the multi page panels
         * if so, pageindex ++ (and loops back to zero) and then we call showinventory again
         */
    }

    public void ShowStarterSelectScreen()
    {
        starterSelectPanel.SetActive(true);
        PetInfo selectedPet;
        starter1Button.onClick.AddListener(delegate
        {
            selectedPet = PetManager.Bulbos;
            MakeStarterSelection(selectedPet);
        });
        starter2Button.onClick.AddListener(delegate
        {
            selectedPet = PetManager.Charby;
            MakeStarterSelection(selectedPet);
        });
        starter3Button.onClick.AddListener(delegate
        {
            selectedPet = PetManager.Squirt;
            MakeStarterSelection(selectedPet);
        });
        EventSystem.current.SetSelectedGameObject(starter1Button.gameObject);
    }

    private void MakeStarterSelection(PetInfo pet)
    {
        PetInfo petToAdd = new PetInfo(pet.petName, pet.Strength.BaseValue, pet.Smarts.BaseValue, pet.Speed.BaseValue, pet.affinity, pet.description, pet.overworldAnimator);
        GameManager.instance.petManager.AddPetToList(petToAdd);
        GameManager.instance.CloseCurrentMenu();
        DialogManager.instance.ShowSimpleDialog("You selected " + pet.petName + " as your starter, congrats!");
    }

    public GameManager.GameState CloseCurrentMenu()
    {
        if (inGameMenuPanel.activeSelf)
        {
            inGameMenuPanel.SetActive(false);
            return GameManager.GameState.NORMAL;
        }
        else if (petInfoPanel.activeSelf)
        {
            petInfoPanel.SetActive(false);
            return GameManager.GameState.NORMAL;
        }
        else if (farmInfoPanel.activeSelf)
        {
            HideFarmInfo();
            ShowIngameMenu();
            return GameManager.GameState.MENU;
        }
        else if (itemsPanel.activeSelf)
        {
            HideItems();
            ShowIngameMenu();
            return GameManager.GameState.MENU;
        }
        else if (resourcesPanel.activeSelf)
        {
            HideResources();
            ShowIngameMenu();
            return GameManager.GameState.MENU;
        }
        else if (foodsPanel.activeSelf)
        {
            HideFoods();
            ShowIngameMenu();
            return GameManager.GameState.MENU;
        }
        else if (regionInfoPanel.activeSelf)
        {
            regionInfoPanel.SetActive(false);
            ShowIngameMenu();
            return GameManager.GameState.MENU;
        }else if(starterSelectPanel.activeSelf)
        {
            starterSelectPanel.SetActive(false);
            return GameManager.GameState.NORMAL;
        }
        return GameManager.GameState.MENU;
    }
}
