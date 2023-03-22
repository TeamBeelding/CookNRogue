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
    
    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
        Debug.Log("SizeOtherDistanceShootEffect");
        //bullet.GetComponent<Transform>().localScale *= m_sizeFactor; //OLD
        ChangeSizeOverDistance _changeSizeOverDistance = bullet.AddComponent<ChangeSizeOverDistance>();
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

    }
}
