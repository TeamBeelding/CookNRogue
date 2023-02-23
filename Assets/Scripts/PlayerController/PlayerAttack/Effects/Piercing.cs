using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piercing : IIngredientEffects
{
    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
        Debug.Log("PiercingShootEffect");
        bullet.GetComponent<PlayerBulletBehaviour>().destroyOnHit = false;
    }


    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        Debug.Log("PiercingHitEffect");

    }
}
