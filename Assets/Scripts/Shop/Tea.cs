using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tea : Item,ISubItem
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
        TeaData data = (TeaData)_data;
        PlayerHealth.instance.Heal(data.regen);
    }
}
