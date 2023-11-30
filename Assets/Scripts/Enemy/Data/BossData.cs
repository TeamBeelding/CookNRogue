using UnityEngine;

[CreateAssetMenu(fileName = "Boss", menuName = "Enemy/Boss")]
public class BossData : ScriptableObject
{
    [Header("Basic")]
    
    [SerializeField] private int bossHealth;
    [SerializeField] private float bossDashSpeed;
    [SerializeField] private float damageOnHitPlayer;
    [SerializeField] private float damageOnHitDash;

    [Space(1f)]
    [Header("Timing")]

    [SerializeField] private float start;
    [SerializeField] private float delayBeforeTeleport;
    [SerializeField] private float delayBeforeTakingLastPlayerPosition;
    [SerializeField] private float castMissiles;
    [SerializeField] private float castDash;

    [Space(1f)]
    [Header("Missiles")]
    
    [SerializeField] private int missilesCount;
    [SerializeField] private float delayBetweenMissiles;
    [SerializeField] private float damagePerMissiles;

    [Space(1f)]
    [Header("Shockwave")]

    [SerializeField] private int shockwaveCount;
    [SerializeField] private float delayBetweenShockwave;
    [SerializeField] private float maxRadius;
    [SerializeField] private float shockwaveDuration;
    [SerializeField] private float damageForShockwave;

    private void Reset()
    {
        bossHealth = 15000;
        bossDashSpeed = 4f;
        damageOnHitPlayer = 1f;
        damageOnHitDash = 1.5f;

        start = 2f;
        delayBeforeTeleport = 2f;
        delayBeforeTakingLastPlayerPosition = 1f;
        castMissiles = 2f;
        castDash = 2f;

        missilesCount = 8;
        delayBetweenMissiles = 0.2f;
        damagePerMissiles = 1f;

        shockwaveCount = 1;
        delayBetweenShockwave = 0;
        maxRadius = 10;
        shockwaveDuration = 3;
        damageForShockwave = 0.5f;
    }

    #region Basic

    public int GetHealth => bossHealth;
    public float GetDashSpeed => bossDashSpeed;
    public float GetDamageOnHitPlayer => damageOnHitPlayer;
    public float GetDamageOnHitDash => damageOnHitDash;

    #endregion

    // --------------------------

    #region Timing

    public float GetStart => start;
    public float GetDelayBeforeTeleport => delayBeforeTeleport;
    public float DelayBeforeTakingLastPlayerPosition => delayBeforeTakingLastPlayerPosition;
    public float GetCastDashDelay => castDash;

    #endregion

    // --------------------------

    #region Missiles

    public int GetMissilesCount => missilesCount;
    public float GetDelayForEachMissiles => delayBetweenMissiles;
    public float GetCastMissilesDelay => castMissiles;

    #endregion

    // --------------------------

    #region Shockwave

    public int GetShockwaveCount => shockwaveCount;
    public float GetDelayForEachShockWave => delayBetweenShockwave;
    public float GetMaxRadius => maxRadius;
    public float GetShockwaveDuration => shockwaveDuration;
    public float GetShockwaveDamage => damageForShockwave;

    #endregion
}
