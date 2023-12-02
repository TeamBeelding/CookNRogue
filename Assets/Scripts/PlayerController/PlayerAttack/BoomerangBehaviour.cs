using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBehaviour : PlayerBulletBehaviour
{
    // Start is called before the first frame update
    public AnimationCurve _forward;
    public AnimationCurve _sides;
    public float _boomerangSpeed;
    private Vector3 _StartPosition;
    private float _progress = 0;
    private float _increment = 0;
    public float _MaxForwardDistance = 10;
    public float _MaxSideDistance = 5;
    Vector3 _Xaxis;
    bool done = false;
    public override void Init()
    {
        _StartPosition = transform.position;
        _increment = _boomerangSpeed/30;
        _Xaxis = Quaternion.Euler(new Vector3(0, 90, 0)) * _direction;
        HasExploded = false;

        PlayerBulletBehaviour[] playerBulletBehaviours = GetComponents<PlayerBulletBehaviour>();
        foreach(PlayerBulletBehaviour playerBulletBehaviour in playerBulletBehaviours)
        {
            if (playerBulletBehaviour != this)
                playerBulletBehaviour.HasExploded = true;
        }
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if(done) return;

        transform.position = _StartPosition + (_direction * _forward.Evaluate(_progress) * _MaxForwardDistance) + (_Xaxis * _sides.Evaluate(_progress) * _MaxSideDistance);
        _progress += _increment;

        if (_progress / _boomerangSpeed >= 1)
        {
            done = true;
            DisableBullet();

        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        //NE PAS ENLEVER
    }


    public override void ResetStats()
    {
        base.ResetStats();
        _progress = 0;
        _increment = 0;
        done = false;
    }


}