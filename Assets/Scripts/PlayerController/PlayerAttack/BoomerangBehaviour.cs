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
    private float _increment = 0;
    public float _MaxForwardDistance = 10;
    public float _MaxSideDistance = 5;
    Vector3 _Xaxis;
    
    protected override void Start()
    {
        _StartPosition = transform.position;
        _boomerangSpeed /= 100;
        _Xaxis = Quaternion.Euler(new Vector3(0,90,0)) * _direction;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        Debug.DrawLine(transform.position, transform.position + (_Xaxis * _MaxSideDistance), Color.red) ;
        float posX = _direction.x * _sides.Evaluate(_increment *_boomerangSpeed);

        transform.position = _StartPosition + (_direction * _forward.Evaluate(_increment * _boomerangSpeed) * _MaxForwardDistance) + (_Xaxis * _sides.Evaluate(_increment * _boomerangSpeed) * _MaxSideDistance);
        if(_increment * _boomerangSpeed > 1)
        {
            DisableBullet();
        }
        _increment++;
        
    }

    protected override void OnDestroy()
    {
        //NE PAS ENLEVER
    }
    protected override void OnTriggerEnter(Collider other)
    {
        //NE PAS ENLEVER
    }


}