using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum INGREDIENT
{
    SALADE,
    VIANDE,
    POISSON
}

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Player/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    public List<IIngredientEffects> _effects;

    [Header("Composition")]
    public INGREDIENT _plat;
    [Space(20)]

    [Header("Inventory")]
    [FormerlySerializedAs("sprite")]
    public Sprite inventorySprite;
    [Space(20)]

    [Header("Physic and Movements")]
    public float _size;
    public float _speed;
    public float _drag;
    [Space(20)]

    [Header("Attack")]
    public int _ammunition;
    public float _attackDelay;
    public float _damage;

    [Space(20)]

    [Header("Debug")]
    public Color color;
}