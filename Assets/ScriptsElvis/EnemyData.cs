using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    [SerializeField]
    private float health = 3;
    [SerializeField]
    public float speed = 5;
    [SerializeField]
    public float range = 4;
    [SerializeField]
    public float attackSpeed = 0.5f;
    [SerializeField]
    public float damage = 1;
}
