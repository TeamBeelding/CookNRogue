using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTest : MonoBehaviour
{
    public GameObject Projectile;
    public Transform muzzle;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject Bullet = Instantiate(Projectile, muzzle.position, Quaternion.identity);
        }
    }
}
