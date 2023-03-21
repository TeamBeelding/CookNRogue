using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : IIngredientEffects
{
    [SerializeField] StatusEffectData _data;


    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
        Debug.Log("StatusEffectDataShootEffect");
    }


    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        if (HitObject.gameObject.GetComponent<EnemyController>())
        {
            HitObject.gameObject.GetComponent<EnemyController>().ApplyEffect(_data);
        }

    }
}
