using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;

    [SerializeField]
    private List<Enemy> EnemiesInLevel;

    public int numOfEnemies;

    [SerializeField] 
    private float timeEnemyDeathCheck;

    [SerializeField]
    private float time;

    public event Action onAllEnnemiesKilled;

    public static EnemyManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }

        _instance = this;
    }

    public void AddEnemyToLevel(Enemy enemy)
    {
        EnemiesInLevel.Add(enemy);
        numOfEnemies++;
    }

    public void RemoveEnemyFromLevel(Enemy enemy)
    {
        EnemiesInLevel.Remove(enemy);
        numOfEnemies--;

        if(numOfEnemies <= 0 && onAllEnnemiesKilled != null)
        {
            onAllEnnemiesKilled();
        }
    }

    public void DestroyAll()
    {
        if (EnemiesInLevel != null) 
        {
            Debug.Log("Enemy Delete");
            for (int i = 0; i < EnemiesInLevel.Count; i++)
            {
                Enemy current = EnemiesInLevel[i];
                RemoveEnemyFromLevel(current);
                Destroy(current.gameObject);
            }
        }
    }
}
