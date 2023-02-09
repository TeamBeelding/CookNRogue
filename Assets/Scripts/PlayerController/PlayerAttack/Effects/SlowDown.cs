using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : IIngredientEffects
{
    [Header("SlowDown")]
    public float SlowDownDuration;
    public float SlowDownPercent;

    //EFFET LORS DU SHOOT
    public void EffectOnShoot()
    {
        Debug.Log("SlowDownShootEffect");
    }

    //EFFET LORS DE LA COLLISION
    public void EffectOnHit()
    {
        Debug.Log("SlowDownHitEffect");
    }
}
