using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeOtherDistance : IIngredientEffects
{

    [Header("SizeOtherDistance")]
    [SerializeField] float _speed;
    [SerializeField] float _minSize;
    [SerializeField] float _maxSize;
    [SerializeField] AnimationCurve _size;
    ChangeSizeOverDistance _changeSizeOverDistance;
    PlayerBulletBehaviour _playerBulletBehaviour;

    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
        Debug.Log("SizeOtherDistanceShootEffect");
        //bullet.GetComponent<Transform>().localScale *= m_sizeFactor; //OLD
        _changeSizeOverDistance = bullet.AddComponent<ChangeSizeOverDistance>();
        _playerBulletBehaviour = bullet.GetComponent<PlayerBulletBehaviour>();
        Keyframe[] keyframes = _size.keys;

        keyframes[0].value = _minSize;
        keyframes[_size.keys.Length - 1].value = _maxSize;
        _size.keys = keyframes;
        _changeSizeOverDistance.SetParameters(_speed, _size);
    }


    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        Debug.Log("SizeOtherDistanceHitEffect");
        Debug.Log(_changeSizeOverDistance.GetCurve().Evaluate(_changeSizeOverDistance.timePassed));
        _playerBulletBehaviour._damage *= (int)_changeSizeOverDistance.GetCurve().Evaluate(_changeSizeOverDistance.timePassed);

    }
}
