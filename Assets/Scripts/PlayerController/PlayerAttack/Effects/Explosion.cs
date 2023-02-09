using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : IIngredientEffects
{
    [Header(" Explosion")]
    public float ExplosionDamage;
    public float ExplosionRadius;

    //EFFET LORS DU SHOOT
    public void EffectOnShoot()
    {
        Debug.Log("ExplosionShootEffect");
    }

    //EFFET LORS DE LA COLLISION
    public void EffectOnHit()
    {
        Debug.Log("ExplosionHitEffect");
    }
}
