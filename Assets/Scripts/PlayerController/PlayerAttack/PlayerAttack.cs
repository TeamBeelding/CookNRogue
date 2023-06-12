using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.InputSystem;


public class PlayerAttack : MonoBehaviour
{
    public GameObject _Projectile;
    public int _ProjectileNbr;
    public int _ammunition;
    private AmmunitionBar _ammunitionBar;
    [HideInInspector]
    public float _TimeBtwShotsRafale;
    PlayerBulletBehaviour _projectileBehaviour;
    public Transform _muzzle;
    //[SerializeField]
    //PlayerKnockback m_knockbackScript;

    
    [Header("Physic and Movements")]
    [HideInInspector] public float _size;
    [HideInInspector] public float _speed;
    [HideInInspector] public float _drag;

    [Header("Attack")]
    private float _defaultShootCooldown;
    public float _shootCooldown;
    public float _damage;
    [SerializeField]private bool _isShooting = false;
    [SerializeReference]
    public List<IIngredientEffects> _effects = new List<IIngredientEffects>();

    bool _asEmptiedAmmo;

    [Header("Sound")]
    [SerializeField] private AK.Wwise.Event _Play_Weapon_Shot;

    bool _shootOnCooldown = false;
    public bool ShootOnCooldown
    {
        get => _shootOnCooldown;
    }

    Coroutine _curShootDelay;

    PlayerController _playerController;
    PlayerCookingInventory _inventory;
    [SerializeField] ParticleSystem _shootingParticles;
    [ColorUsage(true, true)]
    public Color _color;

    [ColorUsage(true, true)]
    [SerializeField] Color defaultcolor;

    private void Start()
    {
        _defaultShootCooldown = _shootCooldown;

        _playerController = GetComponent<PlayerController>();
        _inventory = PlayerCookingInventory.Instance;

        _ammunitionBar = AmmunitionBar.instance;

        if (_ammunitionBar)
        {
            _ammunitionBar.InitAmmoBar(0);
        }

        ResetParameters();
    }



    public void SetIsShooting(bool isShooting)
    {
        _isShooting = isShooting;
    }
    public void Shoot()
    {
        if (!_playerController._isAiming)
            return;

        if (_shootOnCooldown)
            return;

        if(_shootingParticles)
            _shootingParticles.Play();

        _shootOnCooldown = true;
        //BulletInstantiate
        StartCoroutine(Shootbullets(_TimeBtwShotsRafale));
        
        //Shoot Bullet
        _curShootDelay = StartCoroutine(ShootDelay(_shootCooldown));
        
        if(!_asEmptiedAmmo)
        {
            _ammunition--;

            if (_ammunition <= 0)
            {
                _asEmptiedAmmo = true;

                //Reset Audio
                foreach (ProjectileData data in _inventory.EquippedRecipe)
                {
                    data.audioState.SetValue();
                }

                ResetParameters();
            }
        }

        if(_ammunitionBar)
            _ammunitionBar.UpdateAmmoBar();
        //Animation
        //m_knockbackScript.StartKnockback();

    }

    public void ResetAmunation()
    {
        _ammunition = 0;
        ResetParameters();
        _ammunitionBar.UpdateAmmoBar();
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
            if (effect != null)
            {
                effect.EffectOnHit(Position, HitObject, direction);
            }
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

    IEnumerator Shootbullets(float time)
    {
        for(int i = 0; i< _ProjectileNbr; i++)
        {
            float totalAngle = 0;
            float incrementAngle = 0;

            foreach (IIngredientEffects effect in _effects)
            {
                if (effect is ConeShots shots)
                {
                    ConeShots coneShots = shots;
                    incrementAngle = coneShots._bulletAngleSpread;
                    totalAngle = incrementAngle;
                }
            }

            int j = 2;
            if (incrementAngle > 0)
            {
                j = 0;
            }
            


            for (int k = j;k < 3; k++)
            {

                GameObject Bullet = Instantiate(_Projectile, _muzzle.position, Quaternion.identity);


                _projectileBehaviour = Bullet.GetComponent<PlayerBulletBehaviour>();
                _projectileBehaviour.ResetStats();
                _projectileBehaviour._playerAttack = this;
                _projectileBehaviour._speed += _speed;
                _projectileBehaviour._drag -= _drag;
                _Play_Weapon_Shot.Post(Bullet);

                if (_effects.Count > 0)
                {
                    _projectileBehaviour._damage = 0;
                }

                _projectileBehaviour._damage += _damage;
                Vector3 direction = Quaternion.Euler(0, totalAngle, 0) * _playerController.PlayerAimDirection;

                if(_color != null)
                    SetGradientInParticle(Bullet, _color);

                if (direction == Vector3.zero)
                    direction = transform.forward;

                _projectileBehaviour._direction = direction;

                

                foreach (IIngredientEffects effect in _effects)
                {
                    if (effect != null)
                    {
                        effect.EffectOnShoot(transform.position, Bullet);
                    }


                    if (effect is Boomerang boomerang)
                    {
                        BoomerangBehaviour boomerangBehaviour = GetComponent<BoomerangBehaviour>();
                        if (boomerangBehaviour == null)
                        {
                            boomerangBehaviour = Bullet.AddComponent<BoomerangBehaviour>();
                        }

                        Boomerang TempEffect = boomerang;
                        boomerangBehaviour.ResetStats();
                        boomerangBehaviour._forward = TempEffect._forward;
                        boomerangBehaviour._MaxForwardDistance = TempEffect._MaxForwardDistance;
                        boomerangBehaviour._sides = TempEffect._sides;
                        boomerangBehaviour._MaxSideDistance = TempEffect._MaxSideDistance;
                        boomerangBehaviour._playerAttack = this;
                        boomerangBehaviour._boomerangSpeed = TempEffect._Speed;
                        boomerangBehaviour._damage += _damage;
                        boomerangBehaviour._direction = Quaternion.Euler(0, totalAngle, 0) * _playerController.PlayerAimDirection;
                    }
                }

                totalAngle -= incrementAngle;
            }
            

            yield return new WaitForSeconds(time);
        }

    }

    void SetGradientInParticle(GameObject bullet, Color color)
    {

        var RenderModule = bullet.transform.GetChild(1).GetChild(1).GetComponent<ParticleSystemRenderer>();
        Material splashmat = Instantiate(RenderModule.sharedMaterials[0]);
        splashmat.SetColor("_Color",color);
        RenderModule.material = splashmat;
        /*
        var ps = bullet.transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>();
        var psMain = ps.main;
        var grad = psMain.startColor.gradient;
        GradientColorKey[] keys = grad.colorKeys;
        keys[0].color = grad.colorKeys[0].color;
        keys[keys.Length - 1].color = color;
        grad.colorKeys = keys;

        psMain.startColor = grad;
        */
        var BubbleRenderModule = bullet.transform.GetChild(1).GetChild(0).GetComponent<ParticleSystemRenderer>();
        Material Bubblemat = Instantiate(BubbleRenderModule.sharedMaterials[0]);
        Bubblemat.SetColor("_EmissionColor", color);
        BubbleRenderModule.material = Bubblemat;

        bullet.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_BaseColor",color);
    }

    public void OnAmmunitionChange()
    {
        _asEmptiedAmmo = false;
    }

    public void ResetParameters()
    {
        _color = defaultcolor;
        _effects.Clear();
        _size = 0;
        _speed = 0;
        _drag = 0;
        _ProjectileNbr = 1;
        _TimeBtwShotsRafale = 0;
        _damage = 0;
        _ammunition = 0;
        _shootCooldown = _defaultShootCooldown;
    }

    public void FixedUpdate()
    {
        if (_isShooting)
        {
            Shoot();
        }
    }
    public void Reset()
    {
        _ProjectileNbr= 1;
        _size = 1;
        _speed = 1;
        _drag = 0;
        _shootCooldown = 0.1f;
        _damage = 1;
        //m_knockbackScript = GameObject.Find("CharacterModel").GetComponent<PlayerKnockback>();
    }

}