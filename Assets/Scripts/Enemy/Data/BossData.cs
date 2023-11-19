using UnityEngine;

[CreateAssetMenu(fileName = "Boss", menuName = "AI")]
public class BossData : ScriptableObject
{
    [Header("Basic")]
    
    [SerializeField] private int bossHealth;

    [Space(1f)]
    [Header("Missiles")]
    
    [SerializeField] private int missilesCount;
    [SerializeField] private float delaiBetweenMissiles;

    [Space(1f)]
    [Header("Shockwave")]

    [SerializeField] private float innerRadius;
    [SerializeField] private float outerRadius;
    [SerializeField] private float shockwaveSpeed;

    public int GetHealth => bossHealth;
    public int GetMissilesCount => missilesCount;
    public float GetDelaiForEachMissiles => delaiBetweenMissiles;
}
