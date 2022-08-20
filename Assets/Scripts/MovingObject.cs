using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingObject : MonoBehaviour {
    protected bool isMoving = false;

    private Rigidbody2D rb2d;
    private Collider2D col2d;
    [SerializeField]
    private float moveSpeed = 3f;
    private float moveMod = 1f;

    private Transform target;
    private Vector2 direction;
    private Vector2 destination;

    protected Vector2 facingVector;

    public LayerMask blockingLayer;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
    }

    public void MoveInDirection(Vector2 newDirection)
    {
        StopMoving();
        direction = newDirection;
        isMoving = true;
    }

    public void MoveToDestination(Vector2 newDestination)
    {
        StopMoving();
        destination = newDestination;
        isMoving = true;
    }

    public void MoveToTarget(Transform newTarget)
    {
        StopMoving();
        target = newTarget;
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
        target = null;
        destination = Vector2.zero;
        direction = Vector2.zero;
    }

    public void SetMoveMod(float newMod)
    {
        moveMod = newMod;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.gameState == GameManager.GameState.NORMAL)
        {
            if (isMoving)
            {
                Vector2 movementVector = Vector2.zero;
                if (target != null)
                {
                    movementVector = CalculateMovementVector(target.position);
                }
                else if (destination != Vector2.zero)
                {
                    movementVector = CalculateMovementVector(destination);
                }
                else if (direction != Vector2.zero)
                {
                    movementVector = CalculateMovementVector(rb2d.position + direction);
                }
                facingVector = (movementVector - rb2d.position).normalized;
                rb2d.MovePosition(rb2d.position + movementVector.normalized * moveSpeed * moveMod * Time.deltaTime);
            }
        }
    }

    private Vector2 CalculateMovementVector(Vector2 newDestination)
    {
        Vector2 targetVector = (newDestination - rb2d.position).normalized;
        Vector2 move = Vector2.zero;

        Vector2[] directions = new Vector2[8];
        float[] interestLevels = new float[8];
        float[] dangerLevels = new float[8];

        directions[0] = Vector2.up;
        directions[1] = new Vector2(1, 1).normalized;
        directions[2] = Vector2.right;
        directions[3] = new Vector2(1, -1).normalized;
        directions[4] = Vector2.down;
        directions[5] = new Vector2(-1, -1).normalized;
        directions[6] = Vector2.left;
        directions[7] = new Vector2(-1, 1).normalized;

        for (int i = 0; i < directions.Length; i++)
        {
            interestLevels[i] = Mathf.Clamp(Vector2.Dot(directions[i], targetVector), 0f, 1f);

            float checkDist = 5f;
            col2d.enabled = false;
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, .25f, directions[i], checkDist, blockingLayer);
            col2d.enabled = true;

            float shortestDist = 5f;

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.distance < shortestDist)
                {
                    shortestDist = hit.distance;
                }
            }

            dangerLevels[i] = ((checkDist) - shortestDist) / 5f;

            move += (directions[i] * (interestLevels[i] - dangerLevels[i]));
        }
        return move.normalized;
    }
}
