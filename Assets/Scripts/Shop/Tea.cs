using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tea : Item,ISubItem
{
    [SerializeReference] TeaData _Tdata;
    public override void Interact(string tag)
    {
        base.Interact(tag);
        ApplyItem();
    }

    public void ApplyItem()
    {
        if (!CanTrigger())
            return;

        ApplyItemRoutine();
        PlayerHealth.instance.Heal(_Tdata.regen);
    }
}
