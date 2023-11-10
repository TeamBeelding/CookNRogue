using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButteredShoes : Item,ISubItem
{
    [SerializeReference] ButteredShoesData _BSdata;
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
