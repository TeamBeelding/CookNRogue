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
    [SerializeField] private float explosionVelocity;
    [SerializeField] private float delaiForExplose;
    [SerializeField] private float damage;

    private void Reset()
    {
        focusPlayerOnCD = true;
        health = 100;
        focusRange = 0;
        speed = 0;
        distanceToPlayerForExplosion = 0;
        explosionRange = 0;
        explosionVelocity = 0;
        delaiForExplose = 0;
        damage = 0;
    }

    public bool FocusPlayerOnCD => focusPlayerOnCD;
    public float Health => health;
    public float FocusRange => focusRange;
    public float Speed => speed;
    public float DistanceToPlayerForExplosion => distanceToPlayerForExplosion;
    public float ExplosionRange => explosionRange;
    public float ExplosionVelocity => explosionVelocity;
    public float DelaiForExplose => delaiForExplose;
    public float Damage => damage;
}
