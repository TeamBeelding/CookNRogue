using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeCauldron : Item
{
    [SerializeReference] LargeCauldronData _LCdata;
    public override void Interact(string tag)
    {
        base.Interact(tag);
        ApplyItem();
    }

    public override void ApplyItem()
    {

    }
}
