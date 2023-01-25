using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "Bullet")]
public class BulletData : ScriptableObject
{
    [SerializeField] 
    private float speed;
    [SerializeField] 
    private float damage;

    public float GetSpeed() => speed;
}
