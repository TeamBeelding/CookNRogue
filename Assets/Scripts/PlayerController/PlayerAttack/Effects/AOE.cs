using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE : IIngredientEffects
{
    [Header(" AOE")]
    public GameObject ZONE;
    public float AOERadius;
    public float AOEDuration;
    public float AOEDamage;
    public float AOETick;

    //EFFET LORS DU SHOOT
    public void EffectOnShoot()
    {
        Debug.Log("AOEShootEffect");
    }

    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position,GameObject HitObject, Vector3 direction)
    {
        GameObject instancedObj = GameObject.Instantiate(ZONE,Position,Quaternion.identity) as GameObject;
        Debug.Log("AOEnHitEffect");
    }
} 

