using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : IIngredientEffects
{

    [Header("Boomerang")]
    public AnimationCurve _forward;
    public AnimationCurve _sides;
    public float _Speed;
    public float _MaxForwardDistance;
    public float _MaxSideDistance;


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
