using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButteredShoes : Item
{
    [SerializeReference] ButteredShoesData _BSdata;
    public override void Interact(string tag)
    {
        base.Interact(tag);
        ApplyItem();
    }

    public override void ApplyItem()
    {

    }
}
