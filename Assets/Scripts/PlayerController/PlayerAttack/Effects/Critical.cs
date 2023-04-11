using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critical : IIngredientEffects
{
    [SerializeField] float _criticalChances;
    private PlayerBulletBehaviour _bulletBehaviour;
    // Start is called before the first frame update
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {

        _bulletBehaviour = bullet.GetComponent<PlayerBulletBehaviour>();
        float rand = Random.Range(0, 1);
        if(rand< _criticalChances)
            _bulletBehaviour._isCritical= true;
    }

    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        

    }
}
