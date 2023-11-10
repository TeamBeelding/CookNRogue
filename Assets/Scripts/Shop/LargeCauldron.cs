using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeCauldron : Item,ISubItem
{
    [SerializeReference] LargeCauldronData _LCdata;
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
    }
}
