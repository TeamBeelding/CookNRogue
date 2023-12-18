using UnityEngine;

[CreateAssetMenu(fileName = "Kamilkaze", menuName = "Enemy/KamilkazeData")]
public class KamilkazeData : ScriptableObject
{
    [SerializeField] private bool focusPlayerOnCD;
    [SerializeField] private float health;
    [SerializeField] private float focusRange;
    [SerializeField] private float speed;
    [SerializeField] private float distanceToPlayerForExplosion;
    [SerializeField] private float explosionRange;
    [SerializeField] private float delayForExplose;
    [SerializeField] private float damage;
    [SerializeField] private float delayAfterExplosion;

    private void Reset()
    {
        focusPlayerOnCD = true;
        health = 100;
        focusRange = 0;
        speed = 0;
        distanceToPlayerForExplosion = 0.5f;
        explosionRange = 0;
        delayForExplose = 0;
        damage = 0;
        delayAfterExplosion = 0.25f;
    }

    public bool FocusPlayerOnCD => focusPlayerOnCD;
    public float Health => health;
    public float FocusRange => focusRange;
    public float Speed => speed;
    public float DistanceToPlayerForExplosion => distanceToPlayerForExplosion;
    public float ExplosionRange => explosionRange;
    public float DelaiForExplose => delayForExplose;
    public float Damage => damage;
    public float DelayAfterExplosion => delayAfterExplosion;
}
