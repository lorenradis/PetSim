using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TamingManager : MonoBehaviour
{
    public static TamingManager instance = null;

    [SerializeField] private GameObject playerControlsPanel;
    [SerializeField] private Button observeButton;
    [SerializeField] private Button offerButton;
    [SerializeField] private Button befriendButton;
    [SerializeField] private Button runButton;

    [SerializeField] private Image[] heartIcons;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private ItemSlot[] itemSlots;
    [SerializeField] private Button closeInventoryButton;

    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TextMeshProUGUI dialogText;

    [SerializeField] private GameObject continueArrow;

    [SerializeField] private GameObject petObject;
    [SerializeField] private Slider anxietySlider;
    [SerializeField] private Slider aggressionSlider;
    private Animator petAnimator;

    private int runFails = 0;

    public enum BattleState { SETUP, IDLE, PLAYERTURN, ENEMYTURN, OFFER, OBSERVE, BEFRIEND, FLEE, DIALOG }
    public BattleState battleState;
    private BattleState previousState;
    private float timeInState = 0f;

    private PetInfo encounteredPet;
    private PlayerInfo playerInfo;

    private bool petIsActing = false;

    private void ChangeState(BattleState newState)
    {
        if(battleState != newState)
        {
            timeInState = 0f;
            previousState = battleState;
            battleState = newState;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Two battle managers");
            Destroy(gameObject);
        }
    }

    private void SetPetInfo(PetInfo petInfo)
    {
        encounteredPet = petInfo;
        petAnimator = petObject.GetComponent<Animator>();
        petAnimator.runtimeAnimatorController = petInfo.battleAnimator;
        anxietySlider.value = petInfo.Anxiety;
    }

    private void SetPlayerInfo(PlayerInfo newPlayerInfo)
    {
        playerInfo = newPlayerInfo;
        UpdatePlayerHealthDisplay();
    }

    private void Update()
    {
        if(GameManager.instance.gameState == GameManager.GameState.BATTLE)
        {
            ManageState();
        }
    }

    private void ManageState()
    {
        switch(battleState)
        {
            case BattleState.SETUP:
                //set all the ui elements up, get the enemy and player infos from the game manager
                SetPetInfo(GameManager.instance.EncounteredPet);
                SetPlayerInfo(GameManager.instance.playerInfo);
                StartCoroutine(DisplayDialog("A wild " + encounteredPet.petName + " approaches!", BattleState.IDLE));
                break;
            case BattleState.IDLE:
                if(!playerControlsPanel.activeSelf && !inventoryPanel.activeSelf)
                {
                    ShowPlayerControls();
                }
                //if player clicks observe, observe
                //if player clicks feed/gift/offer - show inventory
                //if player clicks befriend, attemptBefriend
                //if player clicks run, attemptrun
                break;
            case BattleState.PLAYERTURN:
                //render the player's chosen action
                break;
            case BattleState.ENEMYTURN:
                //enemy can - do nothing, attack player, run from player
                if(!petIsActing)
                {
                    StartCoroutine(RenderPetTurn());
                }
                break;
            case BattleState.OFFER:
                break;
            case BattleState.OBSERVE:
                break;
            case BattleState.BEFRIEND:
                break;
            default:
                break;
        }
    }

    public void ShowPlayerControls()
    {
        playerControlsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(observeButton.gameObject);
    }

    public void HidePlayerControls()
    {
        playerControlsPanel.SetActive(false);
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
        bool hasItem = false;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].gameObject.SetActive(false);
        }
        int slotIndex = 0;
        foreach(Item food in GameManager.instance.itemManager.allFoods)
        {
            if(food.quantity > 0)
            {
                hasItem = true;
                itemSlots[slotIndex].gameObject.SetActive(true);
                itemSlots[slotIndex].SetItem(food);
                slotIndex++;
            }
        }
        if(hasItem)
        {
            EventSystem.current.SetSelectedGameObject(itemSlots[0].gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(closeInventoryButton.gameObject);
        }
    }

    public void HideInventory()
    {
        inventoryPanel.SetActive(false);
    }

    public void AttemptObservePet()
    {
        HidePlayerControls();
        StartCoroutine(RenderObservationAttempt());
    }

    private IEnumerator RenderObservationAttempt()
    {
        yield return DisplayDialog("You keep very still as you observe the " + encounteredPet.petName + "...", BattleState.OBSERVE);
        int roll = Random.Range(1, 6) + Random.Range(1, 6) + Random.Range(1, 6);
        int critChance = 5;
        int critRoll = Random.Range(1, 100);
        bool didCrit = critRoll <= critChance;
        if (didCrit)
        {
            yield return DisplayDialog("WOW! It's a CRITICAL observation!", BattleState.OBSERVE);
            roll *= 2;
        }

        if(roll < 8)
        {
            yield return DisplayDialog("You were only able to learn a little by observing...", BattleState.OBSERVE);
        }else if(roll < 15)
        {
            yield return DisplayDialog("You learned a decent amount.", BattleState.OBSERVE);
        }
        else
        {
            yield return DisplayDialog("You learned a ton!", BattleState.OBSERVE);
        }

        bool didLevel = GameManager.instance.petManager.IncreasePetKnowledge(encounteredPet.petName, roll);

        if(didLevel)
        {
            yield return DisplayDialog("You learned something new about the pet!", BattleState.OBSERVE);
            yield return DisplayDialog(GameManager.instance.petManager.GetPetKnowledge(encounteredPet.petName), BattleState.OBSERVE);
        }

        int newAnxiety = Random.Range(-10, 10);
        ChangePetAnxiety(newAnxiety);
        int newAggression = Random.Range(-10, 10);
        ChangePetAggression(newAggression);
        ChangeState(BattleState.ENEMYTURN);
    }

    public void OfferFood()
    {
        HideInventory();
        HidePlayerControls();
        ChangeState(BattleState.OFFER);
        StartCoroutine(RenderFoodOffer(GameManager.instance.itemManager.SelectedItem));
    }

    private IEnumerator RenderFoodOffer(Item food)
    {
        food.quantity--;
        int amount = food.foodPoints;
        yield return DisplayDialog("You offer the " + food.itemName + " to the pet...", BattleState.OFFER);
        if(encounteredPet.hatedFood == food)
        {
            amount *= -3;
            yield return DisplayDialog("But they shrink back in disgust, they must hate " + food.itemName + "!", BattleState.OFFER);
        }else if(encounteredPet.lovedFood == food)
        {
            amount *= 3;
            yield return DisplayDialog("And they hungrily gobble it up, they must really love " + food.itemName + "!", BattleState.OFFER);
        }
        else
        {
            yield return DisplayDialog("And they gingerly accept, they look a little calmer now.", BattleState.OFFER);
        }

        ChangePetAnxiety(-amount);
        ChangePetAggression(-amount);

        ChangeState(BattleState.ENEMYTURN);
    }

    public void AttemptBefriendPet()
    {
        HidePlayerControls();
        ChangeState(BattleState.BEFRIEND);
        int roll = Random.Range(0, 100) + GameManager.instance.playerInfo.tameFails * 10;
        Debug.Log("Tame attempt roll was " + roll);
        if(roll >= encounteredPet.Anxiety + encounteredPet.Aggression)
        {
            StartCoroutine(RenderBefriendAttempt(true));
            GameManager.instance.playerInfo.tameFails = 0;
        }
        else
        {
            GameManager.instance.playerInfo.tameFails++;
            Debug.Log("Failed to tame " + GameManager.instance.playerInfo.tameFails + " times");
            StartCoroutine(RenderBefriendAttempt(false));
            int newAnxiety = Random.Range(0, 20);
            int newAggression = Random.Range(0, 20);
            ChangePetAnxiety(newAnxiety);
            ChangePetAggression(newAggression);
        }
    }

    private IEnumerator RenderBefriendAttempt(bool succeeds)
    {

        yield return DisplayDialog("You gently stretch out your hand towards the wild " + encounteredPet.petName + "...", BattleState.BEFRIEND);

        if (succeeds)
        {
            yield return DisplayDialog("...And it nuzzles your outstretched hand!  You've successfully befriended the " + encounteredPet.petName + "!", BattleState.BEFRIEND);
            if (GameManager.instance.petManager.AddPetToList(encounteredPet))
            {
                if (GameManager.instance.petManager.PartnerPet1 == null || GameManager.instance.petManager.PartnerPet2 == null)
                {
                    GameManager.instance.petManager.SetPartnerPet(encounteredPet);
                    yield return DisplayDialog("The " + encounteredPet.petName + " joins your party, welcome aboard " + encounteredPet.petName + "!", BattleState.BEFRIEND);
                }
                else
                {
                    yield return DisplayDialog("The " + encounteredPet.petName + " heads back to the farm, see you soon " + encounteredPet.petName + "!", BattleState.BEFRIEND);
                }
            }
            else
            {
                yield return DisplayDialog("You don't have enough room for more pets, we'll have to say goodbye to " + encounteredPet.petName + " for now... ", BattleState.BEFRIEND);
            }

            GameManager.instance.ReturnFromBattle();

            yield break;
        }
        else
        {
            yield return DisplayDialog("...But it backs away nervously, it's not ready to join you yet.", BattleState.BEFRIEND);
            ChangeState(BattleState.ENEMYTURN);
        }
    }

    public void AttemptRunFromBattle()
    {
        HidePlayerControls();
        StartCoroutine(RenderRunFromBattle());
    }

    private IEnumerator RenderRunFromBattle()
    {
        ChangeState(BattleState.FLEE);
        yield return DisplayDialog("You fled the encounter!", BattleState.FLEE);
        GameManager.instance.ReturnFromBattle();
    }

    private void ChangePlayerHealth(int amount)
    {
        if(amount > 0)
        {
            playerInfo.IncreaseHealth(amount);
        }
        else
        {
            playerInfo.DecreaseHealth(Mathf.Abs(amount));
        }
        UpdatePlayerHealthDisplay();
    }

    private void UpdatePlayerHealthDisplay()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if(i < playerInfo.maxHealth)
            {
                heartIcons[i].enabled = true;
                heartIcons[i].sprite = i < playerInfo.health ? fullHeart : emptyHeart;
            }
            else
            {
                heartIcons[i].enabled = false;
            }
        }
    }

    private void ChangePetAnxiety(int amount)
    {
        encounteredPet.Anxiety += amount;
        //update the anxiety slider
        anxietySlider.value = encounteredPet.Anxiety;
    }

    private void ChangePetAggression(int amount)
    {
        encounteredPet.Aggression += amount;
        //update the anxiety slider
        aggressionSlider.value = encounteredPet.Aggression;
    }

    private IEnumerator RenderPetTurn()
    {
        petIsActing = true;
        int roll = Random.Range(0, 255);
        Debug.Log("Roll is " + roll + ", Anxiety is " + encounteredPet.Anxiety + ", and aggression is " + encounteredPet.Anxiety);
        if(roll < encounteredPet.Anxiety)
        {
            runFails++;
            Debug.Log("pet has considered fleeing " + runFails + " times");
            if (runFails > 2)
            {            //pet runs
                yield return DisplayDialog("The " + encounteredPet.petName + " nervously glances to the side... and makes a break for it!", BattleState.ENEMYTURN);
                GameManager.instance.ReturnFromBattle();
                yield break;
            }
        }else if(roll < encounteredPet.Aggression)
        {
            yield return DisplayDialog("The " + encounteredPet.petName + " lashes out at you!", BattleState.ENEMYTURN);
            ChangePlayerHealth(-encounteredPet.playerDamageAmount);
            if(playerInfo.health < 1)
            {
                yield return DisplayDialog("You've taken too much damage to continue, your vision fades to black as you lose consciousness...", BattleState.ENEMYTURN);
                GameManager.instance.ReturnFromFainting();
                yield break;
            }
            else
            {
                yield return DisplayDialog("You took " + encounteredPet.playerDamageAmount + " damage!", BattleState.ENEMYTURN);
            }

        }
        else
        {
            yield return DisplayDialog("The " + encounteredPet.petName + " is thinking things over", BattleState.ENEMYTURN);
            //pet abides
        }
        ChangeState(BattleState.IDLE);
        petIsActing = false;
    }

    private IEnumerator DisplayDialog(string message, BattleState nextState)
    {
        ChangeState(BattleState.DIALOG);

        dialogPanel.SetActive(true);

        float waitTime = .025f;
        float speedup;
        dialogText.text = message;
        dialogText.maxVisibleCharacters = 0;
        for (int i = 0; i < message.Length+1; i++)
        {
            dialogText.maxVisibleCharacters = i;
            speedup = Input.anyKey ? .5f : 1f;
            yield return new WaitForSeconds(waitTime * speedup);
        }

        continueArrow.SetActive(true);

        bool anyKey = false;
        while(!anyKey)
        {
            anyKey = Input.anyKeyDown;
            yield return null;
        }

        dialogPanel.SetActive(false);

        ChangeState(nextState);
    }
}