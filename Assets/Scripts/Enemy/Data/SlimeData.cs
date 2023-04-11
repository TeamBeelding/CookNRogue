using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SlimeData", menuName = "Enemy/SlimeData")]
public class SlimeData : ScriptableObject
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _attackRange = 4f;
    [SerializeField] private float _attackSpeed = 2f;
    [SerializeField] private float _health = 10f;
    [SerializeField] private bool _focusPlayer = false;
    [SerializeField] private float _focusRange = 10f;
    [SerializeField] private int _slimeSpawnWhenDying = 4;
    
    [SerializeField] private float _radiusMinimoyzSpawnPoint = 0.5f;
    [SerializeField] private float _minimumDistanceToKeep = 2f;

    private void Reset()
    {
        _speed = 2f;
        _attackRange = 4f;
        _attackSpeed = 2f;
        _health = 10f;
        _focusPlayer = false;
        _focusRange = 10f;
        _slimeSpawnWhenDying = 4;
        _radiusMinimoyzSpawnPoint = 0.5f;
        _minimumDistanceToKeep = 2f;
    }

    public float GetSpeed => _speed;
    public float GetAttackRange => _attackRange;
    public float GetAttackSpeed => _attackSpeed;
    public float GetHealth => _health;
    public bool GetFocusPlayer => _focusPlayer;
    public float GetFocusRange => _focusRange;
    public float GetSlimeSpawnWhenDying => _slimeSpawnWhenDying;
    public float GetRadiusMinimoyzSpawnPoint => _radiusMinimoyzSpawnPoint;
    public float GetMinimumDistanceToKeep => _minimumDistanceToKeep;
}
