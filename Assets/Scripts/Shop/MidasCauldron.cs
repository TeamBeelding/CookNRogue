using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidasCauldron : Item, ISubItem
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
        MidasCauldronData data = (MidasCauldronData)_data;
        ItemsBar.Instance.AddItem(data);
        PlayerRuntimeData.GetInstance().data.InventoryData.MidasCauldron = true;
    }
}
