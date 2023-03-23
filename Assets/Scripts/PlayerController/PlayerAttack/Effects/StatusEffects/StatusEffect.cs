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
        
        Debug.Log("StatusEffectDataHitEffect");

        if (!HitObject)
            return;


        if (HitObject.gameObject.GetComponent<EnemyController>())
        {
            StatusEffectHandler handler = HitObject.AddComponent<StatusEffectHandler>();
            handler.ApplyEffect(_data, HitObject.gameObject.GetComponent<EnemyController>());
            HitObject.gameObject.GetComponent<EnemyController>().ApplyEffect(handler);
        }

    }
}
