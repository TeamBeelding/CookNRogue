using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.InputSystem;


public class PlayerAttack : MonoBehaviour
{
    public GameObject _Projectile;

    private AmmunitionBar _ammunitionBar;
    PlayerBulletBehaviour _projectileBehaviour;
    public Transform _muzzle;

    public bool PauseAmmoTimer
    {
        get => _pauseAmmoTimer;
        set
        {
            _pauseAmmoTimer = value;
        }
    }
    //[SerializeField]
    //PlayerKnockback m_knockbackScript;

    [SerializeField] private bool _isShooting = false;

    [SerializeReference] bool _hasEmptiedAmmo = true;

    [Header("Sound")]
    [SerializeField] private AK.Wwise.Event _Play_Weapon_Shot;
    [SerializeField] private AK.Wwise.Event _Play_Weapon_Empty;

    bool _shootOnCooldown = false;
    Coroutine _ammoTimer;
    bool _pauseAmmoTimer;

    public bool ShootOnCooldown
    {
        get => _shootOnCooldown;
    }

    Coroutine _curShootDelay;

    PlayerController _playerController;
    PlayerCookingInventory _inventory;
    [SerializeField] ParticleSystem _shootingParticles;

    private void Start()
    {
        PlayerRuntimeData.GetInstance().data.AttackData.AttackDefaultCooldown = PlayerRuntimeData.GetInstance().data.AttackData.AttackCooldown;

        _playerController = GetComponent<PlayerController>();
        _inventory = PlayerCookingInventory.Instance;

        _ammunitionBar = AmmunitionBar.instance;

        if (_ammunitionBar)
        {
            _ammunitionBar.InitAmmoBar();
        }

        ResetParameters();
    }



    public void SetIsShooting(bool isShooting)
    {
        _isShooting = isShooting;
    }
    public void Shoot()
    {
        //SI LE JOUEUR N'EST PAS EN TRAIN DE VISER
        if (!_playerController._isAiming)
            return;

        //SI LE COOLDOWN DE TIR EST TOUJOURS ACTIF
        if (_shootOnCooldown)
            return;

        if (_shootingParticles)
            _shootingParticles.Play();

        _shootOnCooldown = true;

        //ON FAIS DEBUTER LA FONCTION DE SHOOT
        StartCoroutine(Shootbullets(PlayerRuntimeData.GetInstance().data.AttackData.TimeBtwShotRafale));

        //ON FAIT DEBUTER LE COOLDOWN DE TIR
        _curShootDelay = StartCoroutine(ShootDelay(PlayerRuntimeData.GetInstance().data.AttackData.AttackCooldown));
    }

    IEnumerator IRecipeAmmoTimer()
    {
        _hasEmptiedAmmo = false;
        if (_ammunitionBar)
        {
            _ammunitionBar.InitAmmoBar();
        }

        while (PlayerRuntimeData.GetInstance().data.AttackData.Ammunition > 0)
        {
            if (_pauseAmmoTimer)
                yield return new WaitForSeconds(Time.deltaTime);

            float ammo = PlayerRuntimeData.GetInstance().data.AttackData.Ammunition;
            ammo -= Time.deltaTime;
            ammo = ammo < 0 ? 0 : ammo;
            PlayerRuntimeData.GetInstance().data.AttackData.Ammunition = ammo;

            if (_ammunitionBar)
                _ammunitionBar.UpdateAmmoBar();

            yield return new WaitForSeconds(Time.deltaTime);
        }

        _hasEmptiedAmmo = true;

        ResetParameters();

        _inventory.EquippedRecipe.Clear();
        _inventory.UpdateEquipedRecipeUI();


        //Reset Audio
        foreach (ProjectileData data in _inventory.EquippedRecipe)
        {
            data.audioState.SetValue();
        }
    }

    public void ResetAmunition()
    {
        if (_ammoTimer != null)
        {
            StopCoroutine(_ammoTimer);
        }
        PlayerRuntimeData.GetInstance().data.AttackData.Ammunition = 0;
        ResetParameters();
        _hasEmptiedAmmo = true;
        _ammunitionBar.UpdateAmmoBar();
        _Play_Weapon_Empty.Post(gameObject);
    }

    public void OnDeathReset()
    {
        if (!_hasEmptiedAmmo)
        {
            _hasEmptiedAmmo = true;

            StopCoroutine(_ammoTimer);

            //Reset Audio
            foreach (ProjectileData data in _inventory.EquippedRecipe)
            {
                data.audioState.SetValue();
            }

            _inventory.EquippedRecipe.Clear();
            _inventory.UpdateEquipedRecipeUI();

            ResetParameters();

            if (_ammunitionBar)
                _ammunitionBar.UpdateAmmoBar();
        }
    }


    #region OnHitEffects
    public void ApplyOnHitEffects(Vector3 Position)
    {
        foreach (IIngredientEffects effect in PlayerRuntimeData.GetInstance().data.AttackData.AttackEffects)
        {
            effect.EffectOnHit(Position, null, Vector3.zero);
        }
    }
    public void ApplyOnHitEffects(Vector3 Position, GameObject HitObject, Vector3 direction)
    {
        foreach (IIngredientEffects effect in PlayerRuntimeData.GetInstance().data.AttackData.AttackEffects)
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
        //NOMBRE DE PROJECTILE A TIER A LA SUITE
        for (int i = 0; i < PlayerRuntimeData.GetInstance().data.AttackData.ProjectileNumber; i++)
        {
            float totalAngle = 0;
            float incrementAngle = 0;


            foreach (IIngredientEffects effect in PlayerRuntimeData.GetInstance().data.AttackData.AttackEffects)
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


            for (int k = j; k < 3; k++)
            {
                #region PlayerBulletBehaviour Init
                GameObject Bullet = Instantiate(_Projectile, _muzzle.position, Quaternion.identity);
                _projectileBehaviour = Bullet.GetComponent<PlayerBulletBehaviour>();
                _projectileBehaviour.ResetStats();
                _projectileBehaviour._playerAttack = this;
                _projectileBehaviour._speed += PlayerRuntimeData.GetInstance().data.AttackData.AttackSpeed;
                _projectileBehaviour._drag -= PlayerRuntimeData.GetInstance().data.AttackData.AttackDrag;
                _Play_Weapon_Shot.Post(Bullet);
                #endregion

                if (PlayerRuntimeData.GetInstance().data.AttackData.AttackEffects.Count > 0)
                {
                    _projectileBehaviour._damage = 0;
                }

                _projectileBehaviour._damage += PlayerRuntimeData.GetInstance().data.AttackData.AttackDamage;
                Vector3 direction = Quaternion.Euler(0, totalAngle, 0) * _playerController.PlayerAimDirection;

                var color = PlayerRuntimeData.GetInstance().data.AttackData.AttackColor;
                if(color != null)
                    SetGradientInParticle(Bullet, color.Value);

                if (direction == Vector3.zero)
                    direction = transform.forward;

                _projectileBehaviour._direction = direction;

                ApplyEffectOnShoot(Bullet, ref totalAngle);

                totalAngle -= incrementAngle;
            }


            yield return new WaitForSeconds(time);
        }

    }

    public void ApplyEffectOnShoot(GameObject Bullet, ref float Angle)
    {
        if (PlayerRuntimeData.GetInstance().data.AttackData.AttackEffects.Count == 0)
            return;

        foreach (IIngredientEffects effect in PlayerRuntimeData.GetInstance().data.AttackData.AttackEffects)
        {
            effect.EffectOnShoot(transform.position, Bullet);

            //SI L'EFFET BOOMERANG EST PRESENT
            if (!(effect is Boomerang boomerang))
                continue;

            #region BoomerangBehaviour Init

            BoomerangBehaviour boomerangBehaviour = Bullet.GetComponent<BoomerangBehaviour>();

            if (boomerangBehaviour == null)
                boomerangBehaviour = Bullet.AddComponent<BoomerangBehaviour>();

            Boomerang TempEffect = boomerang as Boomerang;
            boomerangBehaviour.ResetStats();
            boomerangBehaviour._forward = TempEffect._forward;
            boomerangBehaviour._MaxForwardDistance = TempEffect._MaxForwardDistance;
            boomerangBehaviour._sides = TempEffect._sides;
            boomerangBehaviour._MaxSideDistance = TempEffect._MaxSideDistance;
            boomerangBehaviour._playerAttack = this;
            boomerangBehaviour._boomerangSpeed = TempEffect._Speed;
            boomerangBehaviour._damage += PlayerRuntimeData.GetInstance().data.AttackData.AttackDamage;
            boomerangBehaviour._direction = Quaternion.Euler(0, Angle, 0) * _playerController.PlayerAimDirection;
            #endregion

        }
    }

    void SetGradientInParticle(GameObject bullet, Color color)
    {

        var RenderModule = bullet.transform.GetChild(1).GetChild(1).GetComponent<ParticleSystemRenderer>();
        Material splashmat = Instantiate(RenderModule.sharedMaterials[0]);
        splashmat.SetColor("_Color", color);
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

        bullet.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color);
    }

    public void OnAmmunitionChange()
    {
        _ammoTimer = StartCoroutine(IRecipeAmmoTimer());
    }

    public void ResetParameters()
    {
        PlayerRuntimeData.GetInstance().data.AttackData.AttackColor = PlayerRuntimeData.GetInstance().data.AttackData.DefaultColor;
        PlayerRuntimeData.GetInstance().data.AttackData.AttackEffects.Clear();
        PlayerRuntimeData.GetInstance().data.AttackData.AttackSize = 0;
        PlayerRuntimeData.GetInstance().data.AttackData.AttackSpeed = 0;
        PlayerRuntimeData.GetInstance().data.AttackData.AttackDrag = 0;
        PlayerRuntimeData.GetInstance().data.AttackData.ProjectileNumber = 1;
        PlayerRuntimeData.GetInstance().data.AttackData.TimeBtwShotRafale = 0;
        PlayerRuntimeData.GetInstance().data.AttackData.AttackDamage = 0;
        PlayerRuntimeData.GetInstance().data.AttackData.Ammunition = 0;
        PlayerRuntimeData.GetInstance().data.AttackData.AttackCooldown = PlayerRuntimeData.GetInstance().data.AttackData.AttackDefaultCooldown;
        _hasEmptiedAmmo = true;
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
        PlayerRuntimeData.GetInstance().data.AttackData.ProjectileNumber = 1;
        PlayerRuntimeData.GetInstance().data.AttackData.AttackSize = 1;
        PlayerRuntimeData.GetInstance().data.AttackData.AttackSpeed = 1;
        PlayerRuntimeData.GetInstance().data.AttackData.AttackDrag = 0;
        PlayerRuntimeData.GetInstance().data.AttackData.AttackCooldown = 0.1f;
        PlayerRuntimeData.GetInstance().data.AttackData.AttackDamage = 1;
        //m_knockbackScript = GameObject.Find("CharacterModel").GetComponent<PlayerKnockback>();
    }

}