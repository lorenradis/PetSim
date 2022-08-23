using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D col2d;
    private Rigidbody2D rb2d;

    public Transform checkSource;

    private Vector2 facingVector = Vector2.down;
    public Vector2 FacingVector { get { return facingVector; } set { facingVector = value; } }
    private Vector2Int targetSquare;

    private float moveSpeed = 0f;
    private float minMoveSpeed = .1f;
    private float maxMoveSpeed = 4f;
    private float moveMod = 1f;

    private float acceleration = 35f;

    private bool isRunning;

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

    public void UnlockTileType(FarmManager.TileState tileState)
    {
        if (!unlockedTiles.Contains(tileState))
        {
            unlockedTiles.Add(tileState);
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        col2d = GetComponent<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();

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
            AttemptInteract();
        }
        if (Input.GetKey("joystick button 1")) //b button
        {
            if (!isRunning)
                StartRunning();
        }
        else if (Input.GetKeyUp("joystick button 1"))
        {
            StopRunning();
        }
        if (Input.GetKeyDown("joystick button 2")) // x button
        {
            GameManager.instance.ShowIngameMenu();
        }
        if (Input.GetKeyDown("joystick button 3")) // y button
        {
            //if on farm, use tool
            SetTerrainType();
        }
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 5"))
        {
            //cycle one tile type to the left
            tileTypeIndex--;
            if (tileTypeIndex < 0)
                tileTypeIndex = unlockedTiles.Count - 1;
            SetCurrentTileType(tileTypeIndex);
        }
        else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 4"))
        {
            //cycle one tile type to the right
            tileTypeIndex++;
            if (tileTypeIndex >= unlockedTiles.Count)
                tileTypeIndex = 0;
            SetCurrentTileType(tileTypeIndex);
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

    private void StartRunning()
    {
        isRunning = true;
        moveMod = 2f;
    }

    private void StopRunning()
    {
        isRunning = false;
        moveMod = 1f;
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

    private void SetTerrainType()
    {
        if (GameObject.FindGameObjectWithTag("Farm") != null)
        {
            GameManager.instance.farmManager.SetTileState(targetSquare.x, targetSquare.y, tileToPlace);
            StartCoroutine(PauseMovement(.2f));

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
            moveSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            moveSpeed -= acceleration * 2f * Time.deltaTime;
        }

        moveSpeed = Mathf.Clamp(moveSpeed, 0f, maxMoveSpeed);

        if (moveSpeed > minMoveSpeed)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb2d.position, rb2d.position + facingVector.normalized, moveSpeed * moveMod * Time.deltaTime);
            rb2d.MovePosition(newPosition);
        }
    }
}