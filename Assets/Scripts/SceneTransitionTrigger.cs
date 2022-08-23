using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionTrigger : MonoBehaviour
{
    public SceneInfo sceneToLoad;

    public int entranceToUse;

    private bool isActive = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive)
        {
            if (collision.CompareTag("Player"))
            {
                isActive = false;
                GameManager.instance.LoadNewScene(sceneToLoad, entranceToUse);
            }
        }
    }
}
