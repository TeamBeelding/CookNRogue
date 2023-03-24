using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : IIngredientEffects
{
    private PlayerBulletBehaviour _bulletBehaviour;
    [SerializeField] float _maxDistance;
    [SerializeField] LayerMask _sphereMask;
    [SerializeField] LayerMask _rayMask;
    [SerializeField] int _ricochetCount;

    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
        _bulletBehaviour = bullet.GetComponent<PlayerBulletBehaviour>();
        _bulletBehaviour.destroyOnHit = false;
        _bulletBehaviour._ricochetNbr = _ricochetCount;
        _bulletBehaviour._sphereMask = _sphereMask;
        _bulletBehaviour._rayMask = _rayMask;
        _bulletBehaviour._maxDistance = _maxDistance;
    }
    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        /*
        if (!HitObject)
            return;

        Debug.Log("RicochetHitEffect");

        Collider[] hitColliders = Physics.OverlapSphere(Position, maxDistance, _sphereMask);
        float closest = 999f;
        float distance = 0f;
        GameObject closestEnemy = HitObject;
        Collider closestCollider = HitObject.GetComponent<Collider>();

        if(HitObject.GetComponent<EnemyController>())
            HitObject.layer = 2;

        foreach (Collider hitCollider in hitColliders)
        {
            
            if (hitCollider.gameObject != HitObject && hitCollider.GetComponent<EnemyController>())
            {
                hitCollider.transform.gameObject.layer = 2;
                distance = Vector3.Distance(hitCollider.gameObject.transform.position, Position);
                
                Vector3 rayDirection = (hitCollider.gameObject.transform.position - Position).normalized;
                RaycastHit hit;
                Physics.Raycast(Position, rayDirection, out hit, _rayMask);
                
                if (distance < closest && !Physics.Raycast(Position, rayDirection, _rayMask))
                {
                    closest = distance;
                    closestEnemy = hitCollider.gameObject;
                


                }
                hitCollider.transform.gameObject.layer = 0;
            }
            
        }
        
        Debug.DrawLine(Position, closestEnemy.transform.position, Color.red, 999);
        HitObject.layer = 0;
        if (closestEnemy != HitObject)
        {
            Debug.Log("ricochet");
            _bulletBehaviour.CancelInvoke("DestroyBullet");
            _bulletBehaviour.Invoke("DestroyBullet",1);
            _bulletBehaviour._direction = (closestEnemy.gameObject.transform.position - HitObject.transform.position).normalized;
            _bulletBehaviour.destroyOnHit = true;
        }
        */
    }
}
