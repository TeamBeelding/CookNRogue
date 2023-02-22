using UnityEngine;

public interface IIngredientEffects
{
    public void EffectOnShoot(Vector3 Position,GameObject bullet);
    public void EffectOnHit(Vector3 Position, GameObject HitObject,Vector3 direction);
}