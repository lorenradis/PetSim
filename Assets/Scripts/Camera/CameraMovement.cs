using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform target;

    [Range(4, 40)] [SerializeField] private float moveSpeed = 14f;
    private float cutsceneMoveSpeed = 2f;

    [SerializeField]
    private Vector2 minBounds;
    [SerializeField]
    private Vector2 maxBounds;

    private Vector2 movementVector;

    private Vector2 offset;

    private enum CameraMode { NORMAL, CUTSCENE }
    private CameraMode cameraMode;

    private enum CutsceneMode { FOLLOW, STILL, MOVE }
    private CutsceneMode cutsceneMode;

    private void Start()
    {
        minBounds = new Vector2(-1000, -1000);
        maxBounds = new Vector2(1000, 1000);
        offset = Vector2.zero;
        if (GameManager.instance.gameState != GameManager.GameState.BATTLE)
        {
            target = GameManager.instance.Player;
            transform.position = target.position;
        }
    }

    private void Update()
    {
        if (target == null && GameManager.instance.gameState != GameManager.GameState.BATTLE)
            target = GameManager.instance.Player;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition;
        Vector3 newPosition;
        if (GameManager.instance.gameState == GameManager.GameState.BATTLE)
            return;
        switch (cameraMode)
        {
            case CameraMode.NORMAL:
                if (target == null)
                    target = GameManager.instance.Player;
                targetPosition = new Vector3(target.position.x, target.position.y, -10f);
                targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
                newPosition = Vector3.Lerp(transform.position, targetPosition + (Vector3)offset, moveSpeed * Time.deltaTime);
                transform.position = newPosition;
                break;
            case CameraMode.CUTSCENE:

                if (target != null)
                {
                    newPosition = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * cutsceneMoveSpeed);
                    transform.position = new Vector3(newPosition.x, newPosition.y, -10f);
                }
                else if (movementVector != Vector2.zero)
                {
                    newPosition = Vector2.MoveTowards(transform.position, transform.position + (Vector3)movementVector, Time.deltaTime * cutsceneMoveSpeed);
                    transform.position = new Vector3(newPosition.x, newPosition.y, -10f);
                }
                break;
            default: break;
        }
    }


    public void SetMinBounds(Vector2 newBounds)
    {
        Vector2 camSizeOffset = new Vector2(Camera.main.orthographicSize * 16f / 9f, Camera.main.orthographicSize);
        minBounds = newBounds + camSizeOffset;
    }

    public void SetMaxBounds(Vector2 newBounds)
    {
        Vector2 camSizeOffset = new Vector2(Camera.main.orthographicSize * 16f / 9f, Camera.main.orthographicSize);
        maxBounds = newBounds - camSizeOffset;
    }

    public void SetMoveSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetFollowMode()
    {
        cameraMode = CameraMode.NORMAL;
    }

    public void SetCutsceneMode()
    {
        cameraMode = CameraMode.CUTSCENE;
    }

    public void SetMovementVector(Vector2 _movementVector)
    {
        movementVector = _movementVector;
    }

    public void SetOffset(float x, float y)
    {
        offset = new Vector2(x, y);
    }
}
