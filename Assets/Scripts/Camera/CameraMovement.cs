using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform target;

    [Range(4, 40)][SerializeField]private float moveSpeed = 14f;

    private Vector2 offset;

    private void Start()
    {
        offset = Vector2.zero;
        target = GameManager.instance.Player;
        transform.position = target.position;
    }

    private void Update()
    {
        if (target == null)
            target = GameManager.instance.Player;
    }

    private void LateUpdate()
    {
        Vector2 newPosition = Vector2.Lerp((Vector2)transform.position, target.position + (Vector3)offset, Time.deltaTime * moveSpeed);
        transform.position = new Vector3(newPosition.x, newPosition.y, -10f);
    }

    public void SetOffset(float x, float y)
    {
        offset = new Vector2(x, y);
    }
}
