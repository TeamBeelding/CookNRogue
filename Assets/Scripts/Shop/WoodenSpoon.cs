using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSpoon : Item
{
    public override void Interact(string tag)
    {
        base.Interact(tag);
        ApplyItem();
    }

    public override void ApplyItem()
    {

    }
}
