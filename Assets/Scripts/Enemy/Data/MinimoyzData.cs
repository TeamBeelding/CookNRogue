using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDashingData", menuName = "Enemy/Minimoyz")]
public class MinimoyzData : ScriptableObject
{
    [SerializeField] private float health = 3;
    [SerializeField] private float speed = 5;
    [SerializeField] private float damage = 1;
    [SerializeField] private float focusRange = 10;
    [SerializeField] private float attackRange = 1;
    [SerializeField] private float attackSpeed = 1;
    [SerializeField] private bool focusPlayer = false;
    
    public float GetHealth() => health;
    public float GetSpeed() => speed;
    public float GetDamage() => damage;
    public float GetFocusRange() => focusRange;
    public float GetAttackRange() => attackRange;
    public float GetAttackSpeed() => attackSpeed;
    public bool GetFocusPlayer() => focusPlayer;
}
