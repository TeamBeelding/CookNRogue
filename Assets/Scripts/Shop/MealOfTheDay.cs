using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MealOfTheDay : Item, ISubItem
{
    [SerializeField] MealOfTheDayData _MOTDdata;
    bool hasTriggered = false;
    public override void Interact(string tag)
    {
        base.Interact(tag);
        ApplyItem();
    }

    public void ApplyItem()
    {
        if (!CanTrigger())
            return;

        ApplyItemRoutine();
        PlayerHealth.instance.UpgradeMaxHealth(_MOTDdata.playerUpgradeHealth);
    }

}
