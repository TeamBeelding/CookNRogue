using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectData", menuName = "Player/StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    [SerializeField] string _name;
    [Header("Duration of effect")]
    public float _lifetime;
    [Space(20)]

    [Header("Damage  and tick")]
    public float _DOTAmount;
    public float _tickSpeed;
    [Space(20)]

    [Header("movement")]
    public float _movementPenalty;
    public bool _immobilisation;
    [Space(20)]

    [Header("attack Penalty")]
    public float _attackSpeedPenalty;
    public bool _canShoot;
    public bool _looseFocus;
    [Space(20)]

    public ParticleSystem _effectpart;
}
