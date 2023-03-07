using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EFFECT
{
    IMMOBILISATION,
    SLOW,
    POLYMORPHE,
    POISON,
    FLAMMES,
    BLIND
}
public class StatusEffect : IIngredientEffects
{


    [Header("StatusEffect")]
    [Space(20)]

    public EFFECT _effect;
    [Space(20)]

    public float _immobilisationDuration;
    [Space(20)]

    public float _slowDuration;
    public float _slowFactor;
    [Space(20)]

    public float _polymorpheDuration;
    [Space(20)]
    public float _poisonDuration;
    public float _poisonTickDamage;
    [Space(20)]

    public float _flammesDamage;
    [Space(20)]

    public float _blindDuration;

    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
        Debug.Log("StatusEffectShootEffect");
    }

    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        Debug.Log("StatusEffectHitEffect");
        switch(_effect)
        {
            case EFFECT.IMMOBILISATION:
                break;
            case EFFECT.SLOW:
                break;
            case EFFECT.POLYMORPHE:
                break;
            case EFFECT.POISON:
                break;
            case EFFECT.FLAMMES:
                break;
            case EFFECT.BLIND:
                break;
            default :
                break;
        }
    }
}

