using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickStarterStoryTrigger : StoryTrigger
{
    public override bool ConditionMet()
    {
        return !GameManager.instance.storyProgression.HasPickedStarter;
    }

    public override void OnConditionMet()
    {
        GameManager.instance.ChooseStarter();
        GameManager.instance.storyProgression.HasPickedStarter = true;
        base.OnConditionMet();
    }
}
