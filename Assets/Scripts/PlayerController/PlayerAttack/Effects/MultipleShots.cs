using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleShots : IIngredientEffects
{

    [Header("DoubleSized")]
    public int _shotNbr;
    public float _TimebtwShots;


    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {

    }


    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {

    }
}
