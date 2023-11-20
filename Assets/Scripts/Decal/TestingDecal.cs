using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class TestingDecal : MonoBehaviour
{
    public Vector3 getPosition() { return transform.position; }

    public virtual void TestMissileDecal()
    {
        MissileDecal decal = MissileDecalManager.instance.GetAvailableDecal();
        decal.Init(Color.red);
        decal.transform.position = getPosition() + new Vector3(Random.Range(-2f, 2f), 1, Random.Range(-2f, 2f));
        decal.transform.rotation = Quaternion.Euler(90, 0, 0);
    }
    private float timer = 1;
    private void Update()
    {
        if(timer > 0)
            timer-= Time.deltaTime;
        else
        {
            timer += 1;
            TestMissileDecal();
        }
    }
}