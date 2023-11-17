using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ErgonomicHandleData", menuName = "ItemData/Ergonomic Handle Data")]
public class ErgonomicHandleData : ItemData
{
    [Range(1f, 3f)]
    public float fireRateBuff;
}
