using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blind : IIngredientEffects
{
    [Header("Blind")]
    public float BlindEffectDuration;

    //EFFET LORS DU SHOOT
    public void EffectOnShoot()
    {
        Debug.Log("BlindShootEffect");
    }

    //EFFET LORS DE LA COLLISION
    public void EffectOnHit()
    {
        Debug.Log("BlindHitEffect");
    }
}