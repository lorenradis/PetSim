using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D col2d;
    private Rigidbody2D rb2d;

    private bool isMoving = false;
    private Vector2 facingVector = Vector2.down;

    private float moveSpeed = 0f;
    private float minMoveSpeed = .1f;
    private float maxMoveSpeed = 6f;
    private float moveMod = 1f;

    private float acceleration = 25f;

    public int playerNumber = 1;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        col2d = GetComponent<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.instance == null || GameManager.instance.gameState == GameManager.GameState.NORMAL)
        {
            ManageInput();
            UpdateAnimator();
        }
    }

    private void UpdateAnimator()
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("inputX", facingVector.x);
        animator.SetFloat("inputY", facingVector.y);
    }

    private void ManageInput()
    {
        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (movementVector != Vector2.zero)
        {
            isMoving = true;
            facingVector = movementVector;
        }
        else
        {
            isMoving = false;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            AttemptInteract();
        }
    }

    private void AttemptInteract()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, facingVector, 2f);
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.transform.GetComponent<Interactable>())
            {
                hit.transform.GetComponent<Interactable>().OnInteract();
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
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