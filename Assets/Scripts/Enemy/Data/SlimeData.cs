using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SlimeData", menuName = "Enemy/SlimeData")]
public class SlimeData : ScriptableObject
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackRange = 4f;
    [SerializeField] private float attackSpeed = 2f;
    [SerializeField] private float health = 10f;
    [SerializeField] private bool focusPlayer = false;
    [SerializeField] private float focusRange = 10f;
    [SerializeField] private int slimeSpawnWhenDying = 4;
    
    [SerializeField] private float radiusMinimoyzSpawnPoint = 0.5f;
    [SerializeField] private float minimumDistanceToKeep = 2f;
    [SerializeField] private float innerRadius = 0.5f;
    [SerializeField] private float outerRadius = 1f;

    private void Reset()
    {
        speed = 2f;
        attackRange = 4f;
        attackSpeed = 2f;
        health = 10f;
        focusPlayer = false;
        focusRange = 10f;
        slimeSpawnWhenDying = 4;
        radiusMinimoyzSpawnPoint = 0.5f;
        minimumDistanceToKeep = 2f;
        innerRadius = 0.5f;
        outerRadius = 1f;
    }

    public float GetSpeed => speed;
    public float GetAttackRange => attackRange;
    public float GetAttackSpeed => attackSpeed;
    public float GetHealth => health;
    public bool GetFocusPlayer => focusPlayer;
    public float GetFocusRange => focusRange;
    public float GetSlimeSpawnWhenDying => slimeSpawnWhenDying;
    public float GetRadiusMinimoyzSpawnPoint => radiusMinimoyzSpawnPoint;
    public float GetMinimumDistanceToKeep => minimumDistanceToKeep;
    public float GetInnerRadius => innerRadius;
    public float GetOuterRadius => outerRadius;
}
