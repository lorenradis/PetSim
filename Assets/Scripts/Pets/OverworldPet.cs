using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverworldPet : MovingObject
{

    public PetInfo petInfo = null;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public bool flipSprite;

    private float sightRadius = 8f;

    public enum PetState { WILD, TAME, FRIEND }
    public PetState petState;

    private enum MoveState { IDLE, WANDER, ALERT, CHASE, FLEE, INVESTIGATE }
    private MoveState moveState;
    private float timeInState = 0f;
    private float actionTimer = 3f;
    private float randomTime;

    private bool isAlert;

    private void ChangeState(MoveState newState)
    {
        if (moveState != newState)
        {
            moveState = newState;
            timeInState = 0f;
            randomTime = Random.Range(-2f, 2f);
        }
    }

    private void ChangePetState(PetState newState)
    {
        if (petState != newState)
        {
            petState = newState;
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (GameManager.instance.gameState == GameManager.GameState.NORMAL || GameManager.instance == null)
        {
            ManageState();
            UpdateAnimator();
        }
    }

    private void UpdateAnimator()
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("inputX", facingVector.x);
        animator.SetFloat("inputY", facingVector.y);
        if (flipSprite)
            spriteRenderer.flipX = facingVector.x > 0;
    }

    /*
     * if at any point player comes in range, roll.  if roll < anxiety then run
     * if roll < aggression then attack
     * else observe (observe state will stay still, face player as player moves.
     * if player moves without sneaking while being observed, anxiety and aggression increase and pet re-rolls
     */

    private void ManageState()
    {
        timeInState += Time.deltaTime;

        switch (petState)
        {
            case PetState.WILD:
                switch (moveState)
                {
                    case MoveState.IDLE:
                        if (PlayerInRange())
                        {
                            AlertOnPlayer();
                        }else if(FoodInRange())
                        {
                            InvestigateFood();
                        }
                        else if (timeInState > (actionTimer + randomTime))
                        {
                            ChooseActionWild();
                        }
                        break;
                    case MoveState.WANDER:
                        if (PlayerInRange())
                        {
                            AlertOnPlayer();
                        }else if(FoodInRange())
                        {
                            InvestigateFood();
                        }
                        else if (timeInState > (actionTimer + randomTime))
                        {
                            StopMoving();
                            ChangeState(MoveState.IDLE);
                        }
                        break;
                    case MoveState.ALERT:
                        if (GameManager.instance.Player.GetComponent<PlayerControls>().IsMoving && !GameManager.instance.Player.GetComponent<PlayerControls>().IsSneaking)
                        {
                            int roll = Random.Range(1, 100);
                            if (roll < petInfo.Aggression)
                            {
                                petInfo.Aggression *= 2;
                                StartChasing(GameManager.instance.Player);
                            }
                            else
                            {
                                petInfo.Anxiety *= 2;
                                StartFleeing(GameManager.instance.Player);
                            }
                        }
                        else if (PlayerOutOfRange())
                        {
                            StopMoving();
                            ChangeState(MoveState.IDLE);
                        }
                        break;
                    case MoveState.CHASE:
                        if (PlayerOutOfRange()) 
                        {
                            StopMoving();
                            ChangeState(MoveState.IDLE);
                        }
                        break;
                    case MoveState.FLEE:
                        if (PlayerOutOfRange()) 
                        {
                            StopMoving();
                            ChangeState(MoveState.IDLE);
                        }
                        break;
                    case MoveState.INVESTIGATE:
                        if(target == null)
                        {
                            StopMoving();
                            ChangeState(MoveState.IDLE);
                        }
                        if(Vector2.Distance(transform.position, target.position) < 1.5f)
                        {
                            InteractWithTarget();
                        }
                        break;
                }

                break;
            case PetState.TAME:
                switch (moveState)
                {
                    case MoveState.IDLE:
                        if (timeInState > (actionTimer + randomTime))
                        {

                            Vector2 newDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                            MoveInDirection(newDirection);
                            SetMoveMod(.5f);
                            ChangeState(MoveState.WANDER);
                        }
                        break;
                    case MoveState.WANDER:
                        if (timeInState > (actionTimer + randomTime))
                        {
                            StopMoving();
                            ChangeState(MoveState.IDLE);
                        }
                        break;
                }
                break;
            case PetState.FRIEND:

                break;
        }

    }

    private void ChooseActionWild()
    {
        //animator.SetTrigger("idle");
        int roll = Random.Range(0, 4); //0 is wander, 1 is sit, 2 is sniff, 3 is just stop moving/stay idle.
        switch (roll)
        {
            case 0:
                animator.SetTrigger("idle");
                Vector2 newDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                MoveInDirection(newDirection);
                SetMoveMod(.5f);
                ChangeState(MoveState.WANDER);
                break;
            case 1:
                animator.SetTrigger("idle1");
                timeInState = 0f;
                break;
            case 2:
                animator.SetTrigger("idle2");
                timeInState = 0f;
                break;
            case 3:
                StopMoving();
                animator.SetTrigger("idle");
                timeInState = 0f;
                break;
            default: break;
        }
    }

    private bool PlayerInRange()
    {
        float playerDist = Vector2.Distance(transform.position, GameManager.instance.Player.position);
        return playerDist < sightRadius;
    }

    private bool PlayerOutOfRange()
    {
        return Vector2.Distance(transform.position, GameManager.instance.Player.position) > sightRadius * 1.5f;
    }

    private bool FoodInRange()
    {
        for (int i = 0; i < GameManager.instance.baitObjects.Count; i++)
        {
            float dist = Vector2.Distance(transform.position, GameManager.instance.baitObjects[i].position);
            if(dist < sightRadius)
            {
                return true;
            }
        }
        return false;
    }

    private void InvestigateFood()
    {
        ChangeState(MoveState.INVESTIGATE);
        for (int i = 0; i < GameManager.instance.baitObjects.Count; i++)
        {
            float dist = Vector2.Distance(transform.position, GameManager.instance.baitObjects[i].position);
            if (dist < sightRadius)
            {
                MoveToTarget(GameManager.instance.baitObjects[i]);
                return;
            }
        }
    }

    private void InteractWithTarget()
    {
        if(target.GetComponent<ItemObject>())
        {
            if(petInfo.hatedFood == target.GetComponent<ItemObject>().thisItem)
            {
                petInfo.Anxiety *= 2;
                petInfo.Aggression *= 2;
            }else if(petInfo.lovedFood == target.GetComponent<ItemObject>().thisItem)
            {
                petInfo.Anxiety /= 2;
                petInfo.Aggression /= 2;
            }
            else
            {
                petInfo.Anxiety -= target.GetComponent<ItemObject>().thisItem.foodPoints;
                petInfo.Aggression -= target.GetComponent<ItemObject>().thisItem.foodPoints;
            }
            Destroy(target.gameObject);
        }
        StopMoving();
        ChangeState(MoveState.IDLE);
    }

    private void AlertOnPlayer()
    {
        animator.SetTrigger("idle");
        ChangeState(MoveState.ALERT);
        animator.SetTrigger("alert");
        int roll = Random.Range(0, 256);
        if (roll < petInfo.Anxiety)
        {
            StartFleeing(GameManager.instance.Player);
        }
        else if (roll < petInfo.Aggression)
        {
            StartChasing(GameManager.instance.Player);
        }
        else
        {
            facingVector = (GameManager.instance.Player.position - transform.position).normalized;
        }
    }

    private void StartChasing(Transform newTarget)
    {
        StartCoroutine(StartChasingRoutine(newTarget));
    }

    private IEnumerator StartChasingRoutine(Transform newTarget)
    {
        //play some kind of alert animation
        yield return new WaitForSeconds(.25f);
        MoveToTarget(newTarget);
        SetMoveMod(1.5f);
    }

    public void StartFleeing(Transform newTarget)
    {
        StartCoroutine(StartFleeingRoutine(newTarget));
    }

    private IEnumerator StartFleeingRoutine(Transform newTarget)
    {
        //play alert animation
        yield return new WaitForSeconds(.25f);

        Vector2 newDirection = (transform.position - newTarget.position).normalized;
        MoveInDirection(newDirection);
        ChangeState(MoveState.FLEE);
    }

    public void SetPetInfo(PetInfo newPet)
    {
        petInfo = newPet;
        if (GetComponent<InteractablePet>())
        {
            GetComponent<InteractablePet>().SetPetInfo(petInfo);
        }
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = petInfo.overworldAnimator;
        SetMoveMod(petInfo.moveMod);
    }

    public void SetPetToWild()
    {
        ChangePetState(PetState.WILD);
    }

    public void SetPetToTame()
    {
        ChangePetState(PetState.TAME);

    }

    public void SetPetToFriend()
    {
        ChangePetState(PetState.FRIEND);

    }
}