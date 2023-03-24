using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class None : IIngredientEffects
{
    
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
        //NO EFFECT
    }


    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        //NO EFFECT
    }

}
