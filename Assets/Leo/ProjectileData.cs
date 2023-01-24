using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FOODTYPE
{
    VIANDE,
    POISSON,
    LEGUME
}

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Player/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    public FOODTYPE food;
    public float mass;
    public float speed;
    public float damage;
    public Color color;
}
