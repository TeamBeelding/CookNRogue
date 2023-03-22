using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectHandler : MonoBehaviour
{
    private float _currentEffectTime = 0;
    private float _NextTickTime = 0;
    [HideInInspector] public StatusEffectData _effectData;
    private EnemyController _enemyController;

    public void ApplyEffect(StatusEffectData Effectdata , EnemyController enemyController)
    {
        _enemyController = enemyController;
        _effectData = Effectdata;
    }
    public void HandleEffect()
    {

        _currentEffectTime += Time.deltaTime;

        if (_currentEffectTime > _effectData._lifetime)
            Destroy(this);

        if (_effectData == null)
            return;

        if (_effectData._DOTAmount != 0 && _currentEffectTime > _NextTickTime)
        {
            Debug.Log("TICK");
            _NextTickTime += _effectData._tickSpeed;
            //TakeDamage(_effectData._DOTAmount); APPLY DAMAGE
        }

    }
}
