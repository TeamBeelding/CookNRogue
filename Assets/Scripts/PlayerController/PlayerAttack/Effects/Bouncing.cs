using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncing : IIngredientEffects
{
    //EFFET LORS DU SHOOT
    [SerializeField] int m_bouncingNbr;
    private PlayerBulletBehaviour bulletBehaviour;
    public GameObject TomatoSplash;
    [SerializeField]
    private AK.Wwise.Event _Play_Weapon_Bounce_Wall;
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {

        bulletBehaviour = bullet.GetComponent<PlayerBulletBehaviour>();
        bulletBehaviour.bouncingNbr = m_bouncingNbr;
    }


    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        if (TomatoSplash)
        {
            GameObject SplashEffect = GameObject.Instantiate(TomatoSplash, Position, Quaternion.identity);
            GameObject.Destroy(SplashEffect, 1);
            _Play_Weapon_Bounce_Wall.Post(bulletBehaviour.gameObject);
        }

    }
}
