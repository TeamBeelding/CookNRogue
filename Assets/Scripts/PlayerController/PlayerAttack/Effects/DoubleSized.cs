using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSized : IIngredientEffects
{

    [Header("DoubleSized")]
    public float sizeFactor;
    

    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
        Debug.Log("DoubleSizedShootEffect");
        bullet.GetComponent<Transform>().localScale *= sizeFactor;
    }

    public void SizeUp(Vector3 Position,GameObject bullet)
    {
        Debug.Log("DoubleSizedShootEffect");
    }

    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        Debug.Log("DoubleSizedHitEffect");

    }
}
