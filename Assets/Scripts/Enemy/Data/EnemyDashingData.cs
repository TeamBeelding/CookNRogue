using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDashingData", menuName = "Enemy/Dashing")]
public class EnemyDashingData : ScriptableObject
{
    [Header("Statistics")]
    [SerializeField]
    private float health = 3;
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private float damage = 1;
    
    [Header("Feedback")]
    [SerializeField] [InfoBox("Time remaining after red line is shown before the enemy dashes")]
    private float timeRemainingForDash = 2;
    [SerializeField] [InfoBox("Time before the enemy can dash again after a dash")]
    private float timeWaitingDash = 3;
    [SerializeField]
    private float timeBeforeShowingRedLine = 0.5f;
    [SerializeField]
    private float timeBeforeLerpRedLine = 1.25f;
    
    public float GetHealth() => health;
    public float GetSpeed() => speed;
    public float GetDamage() => damage;
    
    /// <summary>
    /// Test
    /// </summary>
    public float GetRemainingForDash() => timeRemainingForDash;
    public float GetTimeWaitingDash() => timeWaitingDash;
    public float GetTimeBeforeShowingRedLine() => timeBeforeShowingRedLine;
    public float GetTimeBeforeLerpRedLine() => timeBeforeLerpRedLine;
}
