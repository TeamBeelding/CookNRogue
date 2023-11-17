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
        PlayerRuntimeData.GetInstance().data.InventoryData.WoodenSpoon = true;
    }


}
