using UnityEngine;

public interface IIngredientEffects
{
    public void EffectOnShoot();
    public void EffectOnHit(Vector3 Position, GameObject HitObject,Vector3 direction);
}