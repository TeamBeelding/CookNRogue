using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSpoon : Item,ISubItem
{
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
