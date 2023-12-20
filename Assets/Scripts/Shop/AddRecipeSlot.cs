using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRecipeSlot : Item, ISubItem
{
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
        //ADD RECIPE SLOT
        PlayerCookingInventory.Instance.IncreaseMaxRecipeSlots();
    }
}
