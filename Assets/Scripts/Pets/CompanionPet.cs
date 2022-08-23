using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionPet : MovingObject
{
    /*
     * what behaviour do we want from the companion?
     * follow the player at a reasonable distance.
     * wander idly when close enough to the player and when player is idle for a certain threshold time?
     * notice and investigate points of interest?
     * a whistle command to summon the pet.
     */

    private float maxDistance = 6f;
    private float minDistance = 1.5f;
    private Transform player;

    private enum PetState { IDLE, FOLLOW, WANDER, INVESTIGATE }
    private PetState petState;
    private float timeInState = 0f;
    private float randomTime;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public bool flipSprite = true;

    private void ChangeState(PetState newState)
    {
        if(petState != newState)
        {
            petState = newState;
            timeInState = 0f;
            randomTime = Random.Range(-2f, 2f);
        }
    }

    private void Start()
    {
        player = GameManager.instance.Player;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ManageState();
        UpdateAnimator();
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
        switch(petState)
        {
            case PetState.IDLE:
                if(CheckPlayerDistance() > maxDistance)
                {
                    FollowPlayer();
                }
                if (timeInState > (timeInState + randomTime))
                {
                    Vector2 newDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                    MoveInDirection(newDirection);
                    SetMoveMod(.5f);
                    ChangeState(PetState.WANDER);
                }
                break;
            case PetState.WANDER:
                if (CheckPlayerDistance() > maxDistance)
                {
                    FollowPlayer();
                }
                if (timeInState > (timeInState + randomTime))
                {
                    StopMoving();
                    ChangeState(PetState.IDLE);
                }
                break;
            case PetState.FOLLOW:
                if(CheckPlayerDistance() < minDistance)
                {
                    ChangeState(PetState.IDLE);
                    StopMoving();
                }
                break;
        }
    
    }

    private float CheckPlayerDistance()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        return dist;
    }

    private void FollowPlayer()
    {
        SetMoveMod(1.5f);
        MoveToTarget(player);
        ChangeState(PetState.FOLLOW);
    }
}
