using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("Statistics")]
    [SerializeField]
    private float health = 3;
    [SerializeField]
    private float speed = 5;
    
    [Header("Attack")]
    [SerializeField]
    private float rangeDetection = 6;
    [SerializeField]
    private float attackRange = 4;
    [SerializeField]
    private float attackSpeed = 0.5f;
    [SerializeField]
    private float damage = 1;
    
    [Header("Physics")]
    [SerializeField] 
    private float forceRecoil = 2;
    
    [Header("State")]
    [SerializeField]
    private bool focusInstantlyPlayer = false;
    
    [Header("Path")]
    [SerializeField]
    private GameObject[] path;

    public float GetHealth() => health;
    public float GetSpeed() => speed;
    public float GetRangeDetection() => rangeDetection;
    public float GetAttackRange() => attackRange;
    public float GetAttackSpeed() => attackSpeed;
    public float GetDamage() => damage;
    public float GetRecoilForce() => forceRecoil;
    public bool GetFocusPlayer() => focusInstantlyPlayer;
}
