using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D col2d;
    private Rigidbody2D rb2d;

    [SerializeField] private GameObject hitEffectPrefab;

    private float attackRadius = .2f;
    private float attackRange = 1f;
    public Transform checkSource;

    [SerializeField] private LayerMask blockingLayer;

    private Vector2 facingVector = Vector2.down;
    public Vector2 FacingVector { get { return facingVector; } set { facingVector = value; } }
    private Vector2Int targetSquare;

    private float moveSpeed = 3.75f;
    private float moveMod = 1f;
    private float runMod = 1.75f;
    private float walkMod = 1f;
    private float sneakMod = .5f;
    private float knockbackForce = 10f;
    private Vector2 knockbackVector;

    private PlayerInfo playerInfo;

    private float acceleration = 35f;

    private bool isRunning;
    private bool isSneaking;
    public bool IsSneaking { get { return isSneaking; } }
    public bool IsMoving { get { return playerState == PlayerState.MOVING; } }

    public int playerNumber = 1;

    public GameObject targetIcon;

    private FarmManager.TileState tileToPlace;
    private int tileTypeIndex;
    private List<FarmManager.TileState> unlockedTiles = new List<FarmManager.TileState>();

    public enum PlayerState { IDLE, MOVING, ANIMATING, KNOCKBACK }
    public PlayerState playerState;
    private PlayerState previousState;
    private float timeInState = 0f;

    private void ChangeState(PlayerState newState)
    {
        if (newState != playerState)
        {
            previousState = playerState;
            playerState = newState;
            timeInState = 0f;
        }
    }



    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        col2d = GetComponent<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        playerInfo = GameManager.instance.playerInfo;
        
        //for debug purposes
        UnlockTileType(FarmManager.TileState.BASIC);
        UnlockTileType(FarmManager.TileState.LONGGRASS);
        UnlockTileType(FarmManager.TileState.DESERT);
        UnlockTileType(FarmManager.TileState.WATER);
    }

    private void Update()
    {
        //        Debug.Log("Player update");
        if (GameManager.instance == null || GameManager.instance.gameState == GameManager.GameState.NORMAL)
        {
            ManageState();
            UpdateAnimator();
        }
    }

    private void UpdateAnimator()
    {
        animator.SetBool("isMoving", playerState == PlayerState.MOVING);
        animator.SetFloat("inputX", facingVector.x);
        animator.SetFloat("inputY", facingVector.y);
    }

    private void ManageState()
    {
        switch (playerState)
        {
            case PlayerState.IDLE:
                ManageInput();
                break;
            case PlayerState.MOVING:
                ManageInput();
                break;
            case PlayerState.ANIMATING:
                break;
            case PlayerState.KNOCKBACK:
                break;
            default: break;
        }
    }

    private void ManageInput()
    {
        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (movementVector != Vector2.zero)
        {
            ChangeState(PlayerState.MOVING);
            facingVector = movementVector;
        }
        else
        {
            ChangeState(PlayerState.IDLE);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) //a button
        {
            Debug.Log("Game state is " + GameManager.instance.gameState + " and we pressed the A button");
            AttemptInteract();
        }
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.E)) //b button
        {            //if on farm with a partner pet, use tool
            PlaceTerrainTile();
        }
        else if (Input.GetKeyUp("joystick button 1"))
        {

        }
        if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.X)) // x button
        {
            GameManager.instance.ShowIngameMenu();
        }
        if (Input.GetKeyDown("joystick button 3") || Input.GetKeyDown(KeyCode.Q)) // y button
        {
            MeleeAttack();
        }
        if (Input.GetKeyDown("joystick button 5") || Input.GetKeyDown(KeyCode.C)) // R button
        {
            //cycle one tile type to the left
            StartSneaking();
            
            tileTypeIndex--;
            if (tileTypeIndex < 0)
                tileTypeIndex = unlockedTiles.Count - 1;
            SetCurrentTileType(tileTypeIndex);
        }else if(Input.GetKeyUp("joystick button 5") || Input.GetKeyUp(KeyCode.C))
        {
            StopSneaking();
        }
        if (Input.GetKeyDown("joystick button 4") || Input.GetKeyDown(KeyCode.Z)) // L button
        {
            //cycle one tile type to the right
            StartRunning();
            tileTypeIndex++;
            if (tileTypeIndex >= unlockedTiles.Count)
                tileTypeIndex = 0;
            SetCurrentTileType(tileTypeIndex);
        }else if(Input.GetKeyUp("joystick button 4") || Input.GetKeyUp(KeyCode.Z))
        {
            StopRunning();
        }
    }

    private void ResetUnlockedTileTypes()
    {
        unlockedTiles.Clear();
    }

    public void UnlockTileType(FarmManager.TileState tileState)
    {
        if (!unlockedTiles.Contains(tileState))
        {
            unlockedTiles.Add(tileState);
        }
    }

    private void SetCurrentTileType(int index)
    {
        tileToPlace = unlockedTiles[index];
        if (GameManager.instance.uiManager != null)
        {
            GameManager.instance.uiManager.UpdateToolDisplay(unlockedTiles, tileTypeIndex);
        }
        //play animation
        //play sound
    }

    private void StartSneaking()
    {
        isSneaking = true;
        moveMod = sneakMod;
    }

    private void StopSneaking()
    {
        isSneaking = false;
        moveMod = walkMod;
    }

    private void StartRunning()
    {
        isRunning = true;
        moveMod = runMod;
    }

    private void StopRunning()
    {
        isRunning = false;
        moveMod = walkMod;
    }

    private void AttemptInteract()
    {
        Debug.Log("Attempting an interaction");
        RaycastHit2D[] hits = Physics2D.RaycastAll(checkSource.position, facingVector, 2f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.GetComponent<Interactable>())
            {
                hit.transform.GetComponent<Interactable>().OnInteract();
                break;
            }
        }
    }

    private IEnumerator PauseMovement(float duration)
    {
        ChangeState(PlayerState.ANIMATING);
        yield return new WaitForSeconds(duration);
        ChangeState(PlayerState.IDLE);
    }

    private void MeleeAttack()
    {
        int meleeEnergy = 5;
        if(playerInfo.HasEnergy(meleeEnergy))
        {
            playerInfo.DecreaseEnergy(meleeEnergy);
            animator.SetFloat("inputX", facingVector.x);
            animator.SetFloat("inputY", facingVector.y);
            animator.SetTrigger("attack");
            StartCoroutine(PauseMovement(5f/12f));
            StartCoroutine(SuccessiveHitChecks());
        }

    }


    private IEnumerator SuccessiveHitChecks()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(checkSource.position, attackRadius, facingVector, attackRange, blockingLayer);
        bool didHit = false;

        foreach(RaycastHit2D hit in hits)
        {
            if (hit.transform.GetComponent<OverworldPet>())
            {
                if (hit.transform.GetComponent<OverworldPet>().petState == OverworldPet.PetState.WILD)
                {
                    didHit = true;
                    //obviously change this later, this is just to see if hitting works
                    Destroy(hit.transform.gameObject);
                }
            }
            else if (hit.transform.GetComponent<GardenObstacleObject>())
            {
                didHit = true;
                GameObject newEffect = Instantiate(hitEffectPrefab, hit.transform.position, Quaternion.identity);
                newEffect.SetActive(true);
                Destroy(newEffect, 1f);
                hit.transform.GetComponent<GardenObstacleObject>().TakeDamage();
                break;
            }
        }
        yield return null;
    }

    public void TakeDamage(int amount)
    {

    }

    private void PlaceTerrainTile()
    {
        if (GameObject.FindGameObjectWithTag("Farm") == null)
            return;

        animator.SetTrigger("touch");

        int terraformCost = 10;
        if (playerInfo.HasEnergy(terraformCost))
        {
            playerInfo.DecreaseEnergy(terraformCost);
            GameManager.instance.farmManager.SetTileState(targetSquare.x, targetSquare.y, tileToPlace);
            StartCoroutine(PauseMovement(.2f));
        }
        else
        {
            DialogManager.instance.ShowSimpleDialog("You don't have the energy to do that right now");
        }

    }

    private void LateUpdate()
    {
        targetSquare = new Vector2Int(Mathf.FloorToInt(checkSource.position.x) + Mathf.FloorToInt(facingVector.x), Mathf.FloorToInt(checkSource.position.y) + Mathf.FloorToInt(facingVector.y));
        targetIcon.transform.position = new Vector2(targetSquare.x, targetSquare.y);
    }

    private void FixedUpdate()
    {
        if (GameManager.instance != null && GameManager.instance.gameState != GameManager.GameState.NORMAL)
            return;

        if (playerState == PlayerState.MOVING)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb2d.position, rb2d.position + facingVector.normalized, moveSpeed * moveMod * Time.deltaTime);
            rb2d.MovePosition(newPosition);
        }else if(playerState == PlayerState.KNOCKBACK)
        {
            rb2d.MovePosition(rb2d.position + knockbackVector * knockbackForce * Time.deltaTime);
        }

    }
}