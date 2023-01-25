using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private BulletData _data;
    
    private bool isDirectionSet = false;
    private float speed;
    private Vector3 direction;

    private void Start()
    {
        speed = _data.GetSpeed();
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (isDirectionSet)
            Move();
    }

    public void SetDirection(Transform dir)
    {
        direction = dir.position - transform.position;
        direction.Normalize();
        isDirectionSet = true;
    }

    private void Move()
    {
        transform.position += direction * (_data.GetSpeed() * Time.deltaTime);
    }
}