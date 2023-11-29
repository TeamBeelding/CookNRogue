using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class TestingDecal : MonoBehaviour
{
    public Vector3 getPosition() { return transform.position; }

    public virtual void TestMissileDecal()
    {
        MissileBoss MISSILE = MissileManager.instance.GetAvailableMissile();
        Vector3 target = new Vector3 (PlayerController.Instance.transform.position.x, 0, PlayerController.Instance.transform.position.z);
        MISSILE.Init(1, target,transform.position);
        //MISSILE.transform.position = getPosition() + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        MISSILE.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private float timer = 3f;
    private void Update()
    {
        if(timer > 0)
            timer-= Time.deltaTime;
        else
        {
            timer += 3f;
            TestMissileDecal();
        }
    }
}