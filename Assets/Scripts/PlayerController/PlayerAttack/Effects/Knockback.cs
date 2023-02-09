using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : IIngredientEffects
{
    [Header("knockback")]
    public float KnockbackForce;

    //EFFET LORS DU SHOOT
    public void EffectOnShoot()
    {
        Debug.Log("KnockbackShootEffect");
    }

    //EFFET LORS DE LA COLLISION
    public void EffectOnHit()
    {
        Debug.Log("KnockbackHitEffect");
    }
}