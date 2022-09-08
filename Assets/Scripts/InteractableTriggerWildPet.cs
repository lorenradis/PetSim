using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTriggerWildPet : InteractableTrigger
{
    public override void OnInteract()
    {
        base.OnInteract();
        GameManager.instance.StartBattleWithPet(gameObject);
    }
}
