using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectData", menuName = "Player/StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    [SerializeField] string _name;
    public float _DOTAmount;
    public float _tickSpeed;
    public float _movementPenalty;
    public float _lifetime;
}
