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

    private enum MoveState { IDLE, WANDER, CHASE, FLEE }
    private MoveState moveState;
    private float timeInState = 0f;
    private float actionTimer = 3f;
    private float randomTime;

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

    private void ManageState()
    {
        timeInState += Time.deltaTime;
        int roll;
        switch (petState)
        {
            case PetState.WILD:
                switch (moveState)
                {
                    case MoveState.IDLE:
                        if (timeInState > (actionTimer + randomTime))
                        {
                            if (PlayerInRange())
                            {
                                roll = Random.Range(1, 100);
                                if (roll > 50)
                                {
                                    StartChasing(GameManager.instance.Player);
                                }
                                else
                                {
                                    StartFleeing(GameManager.instance.Player);
                                }
                            }
                            else
                            {
                                Vector2 newDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                                MoveInDirection(newDirection);
                                SetMoveMod(.5f);
                                ChangeState(MoveState.WANDER);
                            }
                        }
                        break;
                    case MoveState.WANDER:
                        if (PlayerInRange())
                        {
                            roll = Random.Range(1, 100);
                            if (roll > 50)
                            {
                                StartChasing(GameManager.instance.Player);
                            }
                            else
                            {
                                StartFleeing(GameManager.instance.Player);
                            }
                        }
                        else
                        {
                            if (timeInState > (actionTimer + randomTime))
                            {
                                StopMoving();
                                ChangeState(MoveState.IDLE);
                            }
                        }

                        break;
                    case MoveState.CHASE:
                        if (!PlayerInRange())
                        {
                            StopMoving();
                            ChangeState(MoveState.IDLE);
                        }
                        break;
                    case MoveState.FLEE:
                        if (!PlayerInRange())
                        {
                            StopMoving();
                            ChangeState(MoveState.IDLE);
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

    private bool PlayerInRange()
    {
        float playerDist = Vector2.Distance(transform.position, GameManager.instance.Player.position);
        return playerDist < sightRadius;
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