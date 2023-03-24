using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : IIngredientEffects
{
    private GameObject _bullet;
    [SerializeField] float maxDistance;
    [SerializeField] LayerMask _mask;

    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
        Debug.Log("RicochetOnShootEffect");
        _bullet = bullet;
        _bullet.GetComponent<PlayerBulletBehaviour>().destroyOnHit = false;
    }
    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        
        if (!HitObject)
            return;

        Debug.Log("RicochetHitEffect");

        Collider[] hitColliders = Physics.OverlapSphere(HitObject.transform.position, maxDistance,_mask);
        float closest = 999f;
        float distance = 0f;
        GameObject closestEnemy = HitObject;
        foreach (var hitCollider in hitColliders)
        {
            
            if (hitCollider.gameObject != HitObject && hitCollider.GetComponent<EnemyController>())
            {
                Debug.Log(hitCollider);
                distance = Vector3.Distance(hitCollider.gameObject.transform.position, HitObject.transform.position);
                
                Vector3 rayDirection = (hitCollider.gameObject.transform.position - HitObject.transform.position).normalized;
                
                if (distance < closest && !Physics.Raycast(HitObject.transform.position, rayDirection, _mask))
                {
                    closest = distance;
                    closestEnemy = hitCollider.gameObject;
                    Debug.DrawLine(HitObject.transform.position, HitObject.transform.position + direction * maxDistance,Color.red,999);
                }
                    
            }
            Debug.Log(closest);
        }

        if (closestEnemy != HitObject)
        {
            _bullet.GetComponent<PlayerBulletBehaviour>().CancelInvoke("DestroyBullet");
            _bullet.GetComponent<PlayerBulletBehaviour>().Invoke("DestroyBullet",1);
            _bullet.GetComponent<PlayerBulletBehaviour>()._direction = (closestEnemy.gameObject.transform.position - HitObject.transform.position).normalized;
            _bullet.GetComponent<PlayerBulletBehaviour>().destroyOnHit = true;
        }
        else
        {
            GameObject.Destroy(_bullet);
        }
    }
}
