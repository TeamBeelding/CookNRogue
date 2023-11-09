using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caramel : Item
{
    [SerializeReference] CaramelData _Cdata;
    public override void Interact(string tag)
    {
        base.Interact(tag);
        ApplyItem();
    }

    public override void ApplyItem()
    {

    }
}
