using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tea : Item
{
    [SerializeReference] TeaData _Tdata;
    public override void Interact(string tag)
    {
        base.Interact(tag);
        ApplyItem();
    }

    public override void ApplyItem()
    {
        PlayerHealth.instance.Heal(_Tdata.regen);
    }
}
