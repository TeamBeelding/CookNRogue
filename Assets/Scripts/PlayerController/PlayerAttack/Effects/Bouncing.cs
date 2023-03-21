using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncing : IIngredientEffects
{
    //EFFET LORS DU SHOOT
    [SerializeField] int m_bouncingNbr;
    private PlayerBulletBehaviour bulletBehaviour;
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
        Debug.Log("BouncingShootEffect");
        bulletBehaviour = bullet.GetComponent<PlayerBulletBehaviour>();
        bulletBehaviour.bouncingNbr = m_bouncingNbr;
    }


    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        Debug.Log("BouncingHitEffect");
    }
}
