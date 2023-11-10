using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErgonomicHandle : Item, ISubItem
{
    [SerializeReference] ErgonomicHandleData _EHdata;
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
    }

}
