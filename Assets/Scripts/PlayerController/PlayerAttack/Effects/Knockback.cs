using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : IIngredientEffects
{
    [Header("knockback")]
    [SerializeField] float m_knockbackForce;

    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
    }

    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position,GameObject HitObject, Vector3 direction)
    {
        if(HitObject != null) {
            if (HitObject.GetComponent<Rigidbody>())
            {
                
                HitObject.GetComponent<Rigidbody>().AddForce((direction.normalized)* m_knockbackForce, ForceMode.Impulse);
            }
        }
        
    }
}