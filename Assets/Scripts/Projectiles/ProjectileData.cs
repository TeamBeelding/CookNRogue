using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAT
{
    PLAT1,
    PLAT2,
    PLAT3
}

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Player/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    [Header("Composition")]
    public PLAT plat;
    public IngredientData[] ingredients;
    public Sprite sprite;
    [Space(20)]

    [Header("RigidBody")]
    public float mass;
    public float size;
    public float speed;
    public float drag;
    [Space(20)]

    [Header("Attack")]
    public float heavyAttackDelay;
    public float heavyDamage;
    public float lightAttackDelay;
    public float lightDamage;
    public Color color;
}
