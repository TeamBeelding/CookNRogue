using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeOtherDistance : IIngredientEffects
{

    [Header("SizeOtherDistance")]
    [SerializeField] float _speed;
    [SerializeField] float _minSize;
    [SerializeField] float _maxSize;
    [SerializeField] AnimationCurve _sizeOtherDistance;
    [SerializeField] float _minDamageFactor;
    [SerializeField] float _maxDamageFactor;
    [SerializeField] AnimationCurve _damageOtherDistance;
    ChangeSizeOverDistance _changeSizeOverDistance;
    PlayerBulletBehaviour _playerBulletBehaviour;

    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
        Debug.Log("SizeOtherDistanceShootEffect");
        //bullet.GetComponent<Transform>().localScale *= m_sizeFactor; //OLD

        //SIZE
        _changeSizeOverDistance = bullet.AddComponent<ChangeSizeOverDistance>();
        _playerBulletBehaviour = bullet.GetComponent<PlayerBulletBehaviour>();
        Keyframe[] sizeKeyFrames = _sizeOtherDistance.keys;
        sizeKeyFrames[0].value = _minSize;
        sizeKeyFrames[_sizeOtherDistance.keys.Length - 1].value = _maxSize;
        _sizeOtherDistance.keys = sizeKeyFrames;
        _changeSizeOverDistance.SetParameters(_speed, _sizeOtherDistance);

        //Damage
        Keyframe[] DamageKeyFrames = _damageOtherDistance.keys;
        DamageKeyFrames[0].value = _minSize;
        DamageKeyFrames[_damageOtherDistance.keys.Length - 1].value = _maxSize;
        _damageOtherDistance.keys = DamageKeyFrames;
    }


    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        Debug.Log("SizeOtherDistanceHitEffect");
        Debug.Log(_changeSizeOverDistance.GetCurve().Evaluate(_changeSizeOverDistance.timePassed));
        _playerBulletBehaviour._damage *= (int)_changeSizeOverDistance.GetCurve().Evaluate(_changeSizeOverDistance.timePassed);

        _playerBulletBehaviour._damage *= _damageOtherDistance.Evaluate(_changeSizeOverDistance.timePassed);

    }
}
