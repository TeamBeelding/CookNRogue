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
    [SerializeField]
    private float lifeTime = 1.0f;

    public float GetSpeed() => speed;
    public float GetLifeTime() => lifeTime;
    public float GetDamage() => damage;
}
