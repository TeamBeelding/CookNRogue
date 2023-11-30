using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButteredShoes : Item,ISubItem
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
        ButteredShoesData data = (ButteredShoesData)_data;
        PlayerRuntimeData.GetInstance().data.InventoryData.ButteredShoes = true;
        PlayerRuntimeData.GetInstance().data.InventoryData.ButteredShoesValue += data.playerSpeedBuff;
    }

    public override bool AlreadyHasUpgrade()
    {
        return PlayerRuntimeData.GetInstance().data.InventoryData.ButteredShoes;
    }
}
