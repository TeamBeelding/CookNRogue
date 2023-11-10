using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caramel : Item,ISubItem
{
    [SerializeReference] CaramelData _Cdata;
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
