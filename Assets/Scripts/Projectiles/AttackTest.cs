using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTest : MonoBehaviour
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


   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject Bullet = Instantiate(Projectile, muzzle.position, Quaternion.identity);
            projectileBehaviour = Bullet.GetComponent<ProjectileBehaviour>();
            projectileBehaviour.speed = speed;
            projectileBehaviour.drag = drag;

        }
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
