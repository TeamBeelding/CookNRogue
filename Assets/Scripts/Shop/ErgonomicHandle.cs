using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErgonomicHandle : Item, ISubItem
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
        ErgonomicHandleData data = (ErgonomicHandleData)_data;
        ItemsBar.Instance.AddItem(data);
        PlayerRuntimeData.GetInstance().data.InventoryData.ErgonomicHandle = true;
        PlayerRuntimeData.GetInstance().data.InventoryData.ErgonomicHandleValue = data.fireRateBuff;
    }

}
