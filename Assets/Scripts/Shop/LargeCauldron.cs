using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeCauldron : Item,ISubItem
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
        LargeCauldronData data = (LargeCauldronData)_data;
        ItemsBar.Instance.AddItem(data);
        PlayerRuntimeData.GetInstance().data.InventoryData.LargeCauldron = true;
        PlayerRuntimeData.GetInstance().data.InventoryData.LargeCauldronValue = data.SecondBuff;
    }
}
