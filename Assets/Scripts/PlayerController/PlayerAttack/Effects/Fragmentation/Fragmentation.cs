using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragmentation : IIngredientEffects
{
    public ParticleSystem _fragParticles;
    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
    }

    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {

        if (_fragParticles)
        {
            ParticleSystem tempfragParticles = ParticleSystem.Instantiate(_fragParticles, Position, Quaternion.identity) as ParticleSystem;
        }

    }
}
