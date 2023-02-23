using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : IIngredientEffects
{

    [Header("Boomerang")]
    public AnimationCurve forward;
    public AnimationCurve sides;
    public float Speed;
    public float MaxForwardDistance;
    public float MaxSideDistance;


    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
        Debug.Log("BoomerangShootEffect");

    }


    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        Debug.Log("BoomerangHitEffect");

    }
}
