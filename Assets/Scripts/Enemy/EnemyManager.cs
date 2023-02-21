using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;

    [SerializeField]
    private List<EnemyController> _enemiesInLevel;

    public int numOfEnemies;

    [SerializeField] 
    private float timeEnemyDeathCheck;

    [SerializeField]
    private float time;

    public event Action OnAllEnnemiesKilled;

    public static EnemyManager Instance
    {
        get { return _instance; }
    }
   
    public EnemyController[] EnemiesInLevel
    {
        get { return _enemiesInLevel.ToArray(); }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }

        _instance = this;
    }

    public void AddEnemyToLevel(EnemyController enemy)
    {
        _enemiesInLevel.Add(enemy);
        numOfEnemies++;
    }

    public void RemoveEnemyFromLevel(EnemyController enemy)
    {
        _enemiesInLevel.Remove(enemy);
        numOfEnemies--;

        if(numOfEnemies <= 0 && OnAllEnnemiesKilled != null)
        {
            OnAllEnnemiesKilled?.Invoke();
        }
    }

    public void DestroyAll()
    {
        if (_enemiesInLevel != null) 
        {
            Debug.Log("Enemy Delete");
            for (int i = 0; i < _enemiesInLevel.Count; i++)
            {
                EnemyController current = _enemiesInLevel[i];
                RemoveEnemyFromLevel(current);
                Destroy(current.gameObject);
            }
        }
    }
}
