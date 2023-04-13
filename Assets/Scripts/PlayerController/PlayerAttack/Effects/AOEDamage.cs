using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamage : MonoBehaviour
{
    List<EnemyController> ennemies = new List<EnemyController>();
    Dictionary<EnemyController, float> _ennemiesTick = new Dictionary<EnemyController, float>();

    [SerializeField] float _timeBtwTicks;
    [SerializeField] float _tickDamage;

    // Update is called once per frame
    void Update()
    {
        if (ennemies.Count == 0 || _tickDamage == 0)
            return;

        foreach(EnemyController ennemy in ennemies)
        {
            _ennemiesTick[ennemy] += Time.deltaTime;

            if(_ennemiesTick[ennemy] >= _timeBtwTicks)
            {
                ennemy.TakeDamage(_tickDamage);
                _ennemiesTick[ennemy] -= _timeBtwTicks;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyController ennemy = other.GetComponent<EnemyController>();

        if (ennemy)
        {
            ennemies.Add(ennemy);
            _ennemiesTick.Add(ennemy, 0);
        }

    }

    private void OnTriggerExit(Collider other)
    {
         EnemyController ennemy = other.GetComponent<EnemyController>();

        if (ennemy)
        {
            _ennemiesTick.Remove(ennemy);
            ennemies.Remove(ennemy);
        }

    }
}
