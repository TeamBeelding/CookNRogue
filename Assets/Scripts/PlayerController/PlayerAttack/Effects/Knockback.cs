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
    public void EffectOnHit(Vector3 Position,GameObject HitObject, Vector3 direction)
    {
        if(HitObject != null) {
            if (HitObject.GetComponent<Rigidbody>())
            {
                
                HitObject.GetComponent<Rigidbody>().AddForce((direction.normalized + Vector3.up)* KnockbackForce, ForceMode.Impulse);
            }
        }
        
        Debug.Log("KnockbackHitEffect");
    }
}