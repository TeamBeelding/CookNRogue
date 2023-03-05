using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level")]
public class LevelData : ScriptableObject
{
    [Header("Statistics")]
    [SerializeField]
    private int amountOfRooms = 10;

    [SerializeField]
    [Range(0, 10)]
    private int EasyLevels = 6;

    [SerializeField]
    [Range(0, 10)]
    private int HardLevels = 4;

    //public float GetAmountOfRooms() => health;
    //public float GetRangeDetection() => rangeDetection;
    //public float GetAttackRange() => attackRange;
    //public float GetAttackSpeed() => attackSpeed;
    //public float GetDamage() => damage;
    //public float GetRecoilForce() => forceRecoil;
    //public bool GetFocusPlayer() => focusInstantlyPlayer;
}
