using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    [SerializeField]
    private float health = 3;
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private float rangeDetection = 6;
    [SerializeField]
    private float attackRange = 4;
    [SerializeField]
    private float attackSpeed = 0.5f;
    [SerializeField]
    private float damage = 1;
    [SerializeField]
    private bool focusInstantlyPlayer = false;

    public float GetHealth() => health;
    public float GetSpeed() => speed;
    public float GetRangeDetection() => rangeDetection;
    public float GetAttackRange() => attackRange;
    public float GetAttackSpeed() => attackSpeed;
    public float GetDamage() => damage;
    public bool GetFocusPlayer() => focusInstantlyPlayer;
}
