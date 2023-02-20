using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject Projectile;
    PlayerBulletBehaviour projectileBehaviour;
    public Transform muzzle;

    [Header("Physic and Movements")]
    public float size;
    public float speed;
    public float drag;
    [Space(20)]

    [Header("Attack")]
    public float heavyAttackDelay;
    public float heavyDamage;
    public float lightAttackDelay;
    public float lightDamage;

    public List<IIngredientEffects> effects = new List<IIngredientEffects>();

    bool _shootOnCooldown;

    [SerializeField]
    float _shootCooldown;

    Coroutine _curShootDelay;

    PlayerController _playerController;

    private void Start()
    {

        _playerController = GetComponent<PlayerController>();
    }

    
    public void Shoot()
    {
        if (_shootOnCooldown)
            return;
        
        //BulletInstantiate
        GameObject Bullet = Instantiate(Projectile, muzzle.position, Quaternion.identity);
        projectileBehaviour = Bullet.GetComponent<PlayerBulletBehaviour>();
        projectileBehaviour.ResetStats();
        projectileBehaviour.playerAttack = this;
        projectileBehaviour.speed += speed;
        projectileBehaviour.drag -= drag;
        projectileBehaviour.lightDamage += lightDamage;
        projectileBehaviour.heavyDamage += heavyDamage;
        projectileBehaviour.direction = _playerController.PlayerAimDirection;

        foreach (IIngredientEffects effect in effects)
        {
            if(effect != null)
                effect.EffectOnShoot(transform.position);
        }



         //Shoot Bullet
         _shootOnCooldown = true;
         _curShootDelay = StartCoroutine(ShootDelay(_shootCooldown));
        
    }

    #region OnHitEffects
    public void ApplyOnHitEffects(Vector3 Position)
    {
        foreach (IIngredientEffects effect in effects)
        {
            effect.EffectOnHit(Position, null,Vector3.zero);
        }
    }
    public void ApplyOnHitEffects(Vector3 Position,GameObject HitObject, Vector3 direction)
    {
        foreach (IIngredientEffects effect in effects)
        {
            if(effect != null)
                effect.EffectOnHit(Position, HitObject, direction);
        }
    }
    #endregion
    IEnumerator ShootDelay(float delay)
    {
        float curTime = 0;
        while (curTime < delay)
        {
            curTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        _shootOnCooldown = false;
    }

    void OnAmmunitionChange()
    {
        StopCoroutine(_curShootDelay);
        _shootOnCooldown = false;
        _shootCooldown = 1f;
    }

    public void ResetParameters()
    {
        effects.Clear();
        size = 0;
        speed = 0;
        drag = 0;


        heavyAttackDelay = 0;
        heavyDamage = 0;
        lightAttackDelay = 0;
        lightDamage = 0;
    }

}