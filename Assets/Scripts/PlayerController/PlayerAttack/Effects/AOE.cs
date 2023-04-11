using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AOE : IIngredientEffects
{
    [Header(" AOE")]
    public GameObject ZONE;
    [SerializeField] float m_AOERadius;
    [SerializeField] float m_AOEDuration;
    [SerializeField] float m_AOEDamage;
    [SerializeField] float m_AOETick;
    [SerializeField] float m_AOESlowEnnemies;
    [SerializeField] float m_AOESSpeedPlayer;
    [SerializeField] LayerMask _mask;

    //EFFET LORS DU SHOOT
    public void EffectOnShoot(Vector3 Position, GameObject bullet)
    {
    }

    //EFFET LORS DE LA COLLISION
    public void EffectOnHit(Vector3 Position,GameObject HitObject, Vector3 direction)
    {
        RaycastHit hit;
        
        if (Physics.Raycast(Position - (direction), Vector3.down,out hit,Mathf.Infinity, _mask))
        {
            //Debug.Log(hit.transform.name);
            GameObject instancedObj = GameObject.Instantiate(ZONE, hit.point, Quaternion.identity) as GameObject;
            instancedObj.transform.localScale = Vector3.one * (m_AOERadius / 2);
            UnityEngine.Object.Destroy(instancedObj, m_AOEDuration);
        }
 
    }
} 

