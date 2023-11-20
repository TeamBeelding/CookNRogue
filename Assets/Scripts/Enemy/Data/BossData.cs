using UnityEngine;

[CreateAssetMenu(fileName = "Boss", menuName = "AI")]
public class BossData : ScriptableObject
{
    [Header("Basic")]
    
    [SerializeField] private int bossHealth;
    [SerializeField] private float bossDashSpeed;
    [SerializeField] private float damageOnHitPlayer;
    [SerializeField] private float damageOnHitDash;
    [SerializeField] private float damagePerMissiles;
    [SerializeField] private float damageForShockwave;

    [Space(1f)]
    [Header("Timing")]

    [SerializeField] private float start;
    [SerializeField] private float delaiBeforeteleport;
    [SerializeField] private float castMissiles;
    [SerializeField] private float castDash;

    [Space(1f)]
    [Header("Missiles")]
    
    [SerializeField] private int missilesCount;
    [SerializeField] private float delaiBetweenMissiles;

    [Space(1f)]
    [Header("Shockwave")]

    [SerializeField] private float innerRadius;
    [SerializeField] private float outerRadius;
    [SerializeField] private float shockwaveSpeed;

    #region Basic

    public int GetHealth => bossHealth;
    public float GetDashSpeed => bossDashSpeed;
    public float GetDamageOnHitPlayer => damageOnHitPlayer;
    public float GetDamageOnHitDash => damageOnHitDash;

    #endregion

    // --------------------------

    #region Timing

    public float GetStart => start;
    public float GetDelayBeforeTeleport => delaiBeforeteleport;
    public float GetCastMissilesDelay => castMissiles;
    public float GetCastDashDelay => castDash;

    #endregion

    // --------------------------

    #region Missiles

    public int GetMissilesCount => missilesCount;
    public float GetDelaiForEachMissiles => delaiBetweenMissiles;

    #endregion

    // --------------------------

    #region Shockwave

    public float GetInnerRadius => innerRadius;
    public float GetOuterRadius => outerRadius;
    public float GetShockwaveSpeed => shockwaveSpeed;

    #endregion
}
