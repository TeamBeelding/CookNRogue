using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSpatule : Item, ISubItem
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
        BigSpatuleData data = (BigSpatuleData)_data;
        ItemsBar.Instance.AddItem(data);
        PlayerRuntimeData.GetInstance().data.InventoryData.BigSpatule = true;
        PlayerRuntimeData.GetInstance().data.InventoryData.BigSpatuleValue += data.bonusSecond;
    }
}
