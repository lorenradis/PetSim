using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedLoot : MonoBehaviour
{

    private float zHeight = 0f;

    private float heightChange;

    private float duration;

private    Vector2 direction;

    private Item item;

    private bool isActive = false;

    private void Start()
    {
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        duration = Random.Range(.75f, 1.125f);
        heightChange = Random.Range(4.5f, 6f);
    }

    public void SetItem(Item item)
    {
        this.item = item;
        GetComponent<SpriteRenderer>().sprite = item.icon;
    }

    void Update()
    {
        if (isActive)
            return;
        if(duration > 0f)
        {
            zHeight = zHeight + heightChange * Time.deltaTime;
            heightChange += -9.8f * Time.deltaTime;
            transform.position = new Vector2(transform.position.x, zHeight);
            transform.position += (Vector3)direction * Time.deltaTime;
            duration -= Time.deltaTime;
        }
        isActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(GameManager.instance.itemManager.AddItem(item, 1))
        {
            Destroy(gameObject);
        }
    }
}
