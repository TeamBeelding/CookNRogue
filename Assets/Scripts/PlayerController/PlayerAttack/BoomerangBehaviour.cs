using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBehaviour : PlayerBulletBehaviour
{
    // Start is called before the first frame update
    public AnimationCurve forward;
    public AnimationCurve sides;
    public float boomerangSpeed;
    private Vector3 StartPosition;
    private float increment = 0;
    public float MaxForwardDistance = 10;
    public float MaxSideDistance = 5;
    Vector3 Xaxis;
    
    protected override void Start()
    {
        StartPosition = transform.position;
        Xaxis = Quaternion.Euler(new Vector3(0,90,0)) * direction;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        Debug.DrawLine(transform.position, transform.position + (Xaxis * MaxSideDistance), Color.red) ;
        float posX = direction.x * sides.Evaluate(increment * boomerangSpeed);

        transform.position = StartPosition + (direction * forward.Evaluate(increment * boomerangSpeed) * MaxForwardDistance) + (Xaxis * sides.Evaluate(increment * boomerangSpeed) * MaxSideDistance);
        if(increment * boomerangSpeed > 1)
        {
            Destroy(gameObject);
        }
        increment++;
        
    }
}