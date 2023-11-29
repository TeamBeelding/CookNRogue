using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class TestingDecal : MonoBehaviour
{
    public Vector3 getPosition() { return transform.position; }

    [Header("DISPLACEMENT")]
    [SerializeField] bool _randomDisplacement;
    [SerializeField] float _displacementAmount;

    public virtual void TestMissileDecal()
    {
        MissileBoss MISSILE = MissileManager.instance.GetAvailableMissile();
        Vector3 target = new Vector3(PlayerController.Instance.transform.position.x, 0, PlayerController.Instance.transform.position.z);

        if (_randomDisplacement)
        {
            float displacementX = Random.Range(-_displacementAmount, _displacementAmount);
            float displacementZ = Random.Range(-_displacementAmount, _displacementAmount);
            target += new Vector3(displacementX, 0, displacementZ);
        }

        
        MISSILE.Init(1, target,transform.position);
        //MISSILE.transform.position = getPosition() + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        MISSILE.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private float timer = 0f;
    [SerializeField] private float _timerBtwMissiles;
    
    private void Start()
    {
        timer = _timerBtwMissiles;
    }
    private void Update()
    {
        if(timer > 0)
            timer-= Time.deltaTime;
        else
        {
            timer += _timerBtwMissiles;
            TestMissileDecal();
        }
    }
}