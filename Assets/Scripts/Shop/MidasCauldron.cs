using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidasCauldron : Item, ISubItem
{
    [SerializeReference] MidasCauldronData _MCdata;
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
