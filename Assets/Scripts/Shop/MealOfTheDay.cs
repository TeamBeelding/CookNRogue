using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MealOfTheDay : Item, ISubItem
{
    bool hasTriggered = false;

    
    public override void Interact(string tag)
    {
        base.Interact(tag);
        TriggerItem();
    }

    public void TriggerItem()
    {
        if (!CanTrigger())
            return;

        _triggerEffect.AddListener(ApplyItem);
        ApplyItemRoutine();
        
    }

    public void ApplyItem()
    {
        MealOfTheDayData data = (MealOfTheDayData)_data;
        ItemsBar.Instance.AddItem(data);

        PlayerHealth.instance.UpgradeMaxHealth(data.playerUpgradeHealth);
        
    }

}
