using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject Projectile;
    ProjectileBehaviour projectileBehaviour;
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

    bool _shootOnCooldown;
    float _shootCooldown;
    Coroutine _curShootDelay;

    PlayerController _playerController;

    private void Start()
    {
        _playerController= GetComponent<PlayerController>();
    }

    public void Shoot()
    {
        GameObject Bullet = Instantiate(Projectile, muzzle.position, Quaternion.identity);
        projectileBehaviour = Bullet.GetComponent<ProjectileBehaviour>();
        projectileBehaviour.speed = speed;
        projectileBehaviour.drag = drag;
        projectileBehaviour.direction = _playerController.PlayerAimDirection; 

        //Shoot Cooldown
        _shootCooldown = 1f; //To get from coocked bullet 

        //Cooldown Check
        if (!_shootOnCooldown)
        {
            //Shoot Bullet
            _shootOnCooldown = true;
            _curShootDelay = StartCoroutine(ShootDelay(_shootCooldown));
        }
    }

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
        size = 0;
        speed = 0;
        drag = 0;


        heavyAttackDelay = 0;
        heavyDamage = 0;
        lightAttackDelay = 0;
        lightDamage = 0;
    }
}
