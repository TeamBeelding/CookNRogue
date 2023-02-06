using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBulletData", menuName = "Bullet")]
public class EnemyBulletData : ScriptableObject
{
    [SerializeField] 
    private float speed;
    [SerializeField] 
    private float damage;

    public float GetSpeed() => speed;
}
