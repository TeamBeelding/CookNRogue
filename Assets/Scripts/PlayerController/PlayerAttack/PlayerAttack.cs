using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject _Projectile;
    public int _ProjectileNbr;
    [HideInInspector]
    public float _TimeBtwShotsRafale;
    PlayerBulletBehaviour _projectileBehaviour;
    public Transform _muzzle;

    [Header("Physic and Movements")]
    public float _size;
    public float _speed;
    public float _drag;
    [Space(20)]

    [Header("Attack")]
    public float _heavyAttackDelay;
    public float _heavyDamage;
    public float _lightAttackDelay;
    public float _lightDamage;

    public List<IIngredientEffects> _effects = new List<IIngredientEffects>();
    
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
        StartCoroutine(shootbullets(_TimeBtwShotsRafale));
        



        

         //Shoot Bullet
         _shootOnCooldown = true;
         _curShootDelay = StartCoroutine(ShootDelay(_shootCooldown));
        
    }

    #region OnHitEffects
    public void ApplyOnHitEffects(Vector3 Position)
    {
        foreach (IIngredientEffects effect in _effects)
        {
            effect.EffectOnHit(Position, null,Vector3.zero);
        }
    }
    public void ApplyOnHitEffects(Vector3 Position,GameObject HitObject, Vector3 direction)
    {
        foreach (IIngredientEffects effect in _effects)
        {
            Debug.Log(HitObject);
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

    IEnumerator shootbullets(float time)
    {
        for(int i = 0; i< _ProjectileNbr; i++)
        {
            GameObject Bullet = Instantiate(_Projectile, _muzzle.position, Quaternion.identity);
            _projectileBehaviour = Bullet.GetComponent<PlayerBulletBehaviour>();
            _projectileBehaviour.ResetStats();
            _projectileBehaviour._playerAttack = this;
            _projectileBehaviour._speed += _speed;
            _projectileBehaviour._drag -=_drag;
            _projectileBehaviour._lightDamage += _lightDamage;
            _projectileBehaviour._heavyDamage += _heavyDamage;
            _projectileBehaviour._direction = _playerController.PlayerAimDirection;

            

            foreach (IIngredientEffects effect in _effects)
            {
                if (effect != null)
                    effect.EffectOnShoot(transform.position, Bullet);

                if (effect is Boomerang)
                {
                    
                    BoomerangBehaviour boomerangBehaviour = GetComponent<BoomerangBehaviour>();
                    if (boomerangBehaviour == null)
                    {
                        boomerangBehaviour = Bullet.AddComponent<BoomerangBehaviour>();
                    }
                    
                    Boomerang TempEffect = (Boomerang)effect;
                    boomerangBehaviour.ResetStats();
                    boomerangBehaviour._forward = TempEffect._forward;
                    boomerangBehaviour._MaxForwardDistance = TempEffect._MaxForwardDistance;
                    boomerangBehaviour._sides = TempEffect._sides;
                    boomerangBehaviour._MaxSideDistance = TempEffect._MaxSideDistance;
                    boomerangBehaviour._playerAttack = this;
                    boomerangBehaviour._boomerangSpeed = TempEffect._Speed;
                    boomerangBehaviour._lightDamage += _lightDamage;
                    boomerangBehaviour._heavyDamage += _heavyDamage;
                    boomerangBehaviour._direction = _playerController.PlayerAimDirection;
                }
            }

            yield return new WaitForSeconds(time);
        }

    }

    void OnAmmunitionChange()
    {
        StopCoroutine(_curShootDelay);
        _shootOnCooldown = false;
        _shootCooldown = 1f;
    }

    public void ResetParameters()
    {
        _effects.Clear();
        _size = 0;
        _speed = 0;
        _drag = 0;
        _ProjectileNbr = 1;
        _TimeBtwShotsRafale = 0;
        _heavyAttackDelay = 0;
        _heavyDamage = 0;
        _lightAttackDelay = 0;
        _lightDamage = 0;
    }

}