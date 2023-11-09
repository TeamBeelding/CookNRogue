using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidasCauldron : Item
{
    [SerializeReference] MidasCauldronData _MCdata;
    public override void Interact(string tag)
    {
        base.Interact(tag);
        ApplyItem();
    }

    public override void ApplyItem()
    {

    }
}
