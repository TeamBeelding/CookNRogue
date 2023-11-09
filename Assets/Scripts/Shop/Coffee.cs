using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : Item
{
    [SerializeReference] CoffeeData _Cdata;
    public override void Interact(string tag)
    {
        base.Interact(tag);
        ApplyItem();
    }

    public override void ApplyItem()
    {
        PlayerHealth.instance.Heal(_Cdata.regen);
    }
}
