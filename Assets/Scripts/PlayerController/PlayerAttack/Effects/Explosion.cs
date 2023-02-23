using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Explosion : IIngredientEffects
{
    [Header(" Explosion")]
    public float ExplosionDamage;
    public float ExplosionRadius;
    public float ExplosionForce;

    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position)
    {
       
        Debug.Log("ExplosionShootEffect");
    }

    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position,  GameObject HitObject, Vector3 direction)
    {
        kaboom(Position);
        Debug.Log("ExplosionHitEffect");
    }

    void kaboom(Vector3 Position)
    {
        

        Collider[] hitColliders = Physics.OverlapSphere(Position, ExplosionRadius);

        foreach(Collider hitCollider in hitColliders)
        {
           
                //float distance = (Position - hitCollider.transform.position).magnitude;
                
                
                Rigidbody rb = hitCollider.GetComponent<Rigidbody>();

                if (!hitCollider.CompareTag("Player") && rb != null)
                {
                Debug.Log("kaboom");
                rb.AddExplosionForce(ExplosionForce, Position, ExplosionRadius);
                }

            
            
        }


    }
}
