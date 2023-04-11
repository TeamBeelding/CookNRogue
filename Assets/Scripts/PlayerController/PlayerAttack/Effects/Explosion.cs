using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Explosion : IIngredientEffects
{
    [Header(" Explosion")]
    [SerializeField] float m_ExplosionDamage;
    [SerializeField] float m_ExplosionRadius;
    [SerializeField] float m_ExplosionForce;

    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
       

    }

    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position,  GameObject HitObject, Vector3 direction)
    {
        kaboom(Position);

    }

    void kaboom(Vector3 Position)
    {
        

        Collider[] hitColliders = Physics.OverlapSphere(Position,m_ExplosionRadius);

        foreach(Collider hitCollider in hitColliders)
        {
           
                //float distance = (Position - hitCollider.transform.position).magnitude;
                
                
                Rigidbody rb = hitCollider.GetComponent<Rigidbody>();

                if (!hitCollider.CompareTag("Player") && rb != null)
                {
                    rb.AddExplosionForce(m_ExplosionForce, Position, m_ExplosionRadius);
                }

            
            
        }


    }
}
