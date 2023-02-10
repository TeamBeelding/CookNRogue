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
    public IIngredientEffects effect;

    [Header("Composition")]
    public INGREDIENT plat;
    [Space(20)]

    [Header("Inventory")]
    [FormerlySerializedAs("sprite")]
    public Sprite inventorySprite;
    [Space(20)]

    [Header("Physic and Movements")]
    public float size;
    public float speed;
    public float drag;
    [Space(20)]

    [Header("Attack")]
    public float heavyAttackDelay;
    public float heavyDamage;
    public float lightAttackDelay;
    public float lightDamage;
    [Space(20)]

    [Header("Dot")]
    public bool Dot;
    public float DotDamage;
    public float DotDuration;

    [Header("ArmorReduction")]
    public bool ArmorReduction;
    public float ArmorReductionForce;

    [Header("Boomerang")]
    public bool BoomerangEffect;

    [Header("Sandwich")]
    public bool SandwichEffect;

    [Header("AOE")]
    public bool AOE;
    public float AOERadius;
    public float AOEDuration;
    public float AOEDamage;
    public float AOETick;
    [Space(20)]

    [Header("Debug")]
    public Color color;
}