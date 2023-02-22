using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleShots : IIngredientEffects
{

    [Header("DoubleSized")]
    public int shotNbr;
    public float TimebtwShots;


    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
        Debug.Log("MultipleShotsShootEffect");

    }


    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        Debug.Log("MultipleShotsdHitEffect");

    }
}
