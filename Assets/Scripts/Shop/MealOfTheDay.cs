using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MealOfTheDay : Item, ISubItem
{
    bool hasTriggered = false;

    protected override void Update()
    {
        base.Update();
    }
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

        for (int i = 0; i < data.playerUpgradeHealth; i++)
        {
            PlayerHealth.instance.UpgradeMaxHealth(data.playerUpgradeHealth);
        }
    }

}
