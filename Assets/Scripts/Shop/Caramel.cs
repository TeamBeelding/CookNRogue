using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caramel : Item,ISubItem
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
        CaramelData data = (CaramelData)_data;
        ItemsBar.Instance.AddItem(data);
        PlayerRuntimeData.GetInstance().data.InventoryData.Caramel = true;
        PlayerRuntimeData.GetInstance().data.InventoryData.CaramelDamage = data.Damage;
    }

    public override bool AlreadyHasUpgrade()
    {
        return PlayerRuntimeData.GetInstance().data.InventoryData.Caramel;
    }
}