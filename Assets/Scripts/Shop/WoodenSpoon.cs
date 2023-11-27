using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSpoon : Item,ISubItem
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

        ApplyItemRoutine();
    }

    public void ApplyItem()
    {
        WoodenSpoonData data = (WoodenSpoonData)_data;
        PlayerRuntimeData.GetInstance().data.InventoryData.ButteredShoes = true;
        PlayerRuntimeData.GetInstance().data.InventoryData.ButteredShoesValue += data.damageBonus;
    }


}
